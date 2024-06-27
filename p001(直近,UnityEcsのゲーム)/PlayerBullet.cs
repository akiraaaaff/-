using P001.Common;
using P001.EcsPaeticle;
using P001.Simulation.Collision;
using P001.Simulation.ORCA;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace P001.SampleScene {
    struct PlayerBulletTag : IComponentData, IEnableableComponent {

        public static void SetPlayerBullet(Entity prototype, EntityManager entityManager) {

            entityManager.AddComponentData(prototype, new PlayerBulletMoveData
            {
                MaxDistance = math.lengthsq(10f),
                Speed = 6f,
            });
            entityManager.AddComponentData(prototype, new PlayerBulletTag());
            entityManager.AddComponentData(prototype, new AttackData());
            entityManager.SetComponentEnabled<PlayerBulletTag>(prototype, false);

            // 伤害过的单位
            entityManager.AddBuffer<HitedUnits>(prototype);

            var shape = new ShapeComponent();
            shape.SetCircleShape(0.5f, 0, Layer.PlayerBullet, Layer.Enemy);
            entityManager.AddComponentData(prototype, shape);
            entityManager.SetComponentEnabled<ShapeComponent>(prototype, false);
        }
    }

    struct PlayerBulletMoveData : IComponentData {
        public float MaxDistance;
        public float Speed;
        public float2 StartPos;
    }

    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SampleSceneSystemGroup))]
    partial struct PlayerBulletMoveSystem : ISystem {

        Job job;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PlayerBulletTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {

            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();

            job.deltaTime = SystemAPI.Time.fixedDeltaTime;
            job.ecbParallel = ecbParallel;
            state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        partial struct Job : IJobEntity {
            public float deltaTime;
            public EntityCommandBuffer.ParallelWriter ecbParallel;

            void Execute(Entity entity, [EntityIndexInQuery] int index,
                ref LocalTransform transform, ref ShapeComponent shape, in PlayerBulletMoveData moveData, ref DynamicBuffer<HitedUnits> hitedUnits) {
                if (math.distancesq(moveData.StartPos, transform.Position.xz) >= moveData.MaxDistance) {
                    ecbParallel.SetComponentEnabled<PlayerBulletTag>(index, entity, false);
                    ecbParallel.SetComponentEnabled<ParticleGroupComponent>(index, entity, false);
                    ecbParallel.SetComponentEnabled<ShapeComponent>(index, entity, false);
                    hitedUnits.Clear();
                    return;
                }

                var pos = transform.Forward() * deltaTime * moveData.Speed;
                transform.Position += pos;
                shape.Position = transform.Position.xz;
            }
        }
    }


    [DisableAutoCreation]
    [BurstCompile]
    public partial class PlayerBulletCollisionSystem : SystemBase {

        ComponentLookup<HpData> hpDataLookup;
        ComponentLookup<HitDirection> hitDirectionLookup;
        ComponentLookup<AnimationData> aniDataLookup;
        Job job;

        public CollisionQuery<AgentComponent> CollisionQuery;

        protected override void OnCreate() {

            hpDataLookup = GetComponentLookup<HpData>(true);
            hitDirectionLookup = GetComponentLookup<HitDirection>(true);
            aniDataLookup = GetComponentLookup<AnimationData>(true);
        }

        protected override void OnUpdate() {

            hpDataLookup.Update(this);
            hitDirectionLookup.Update(this);
            aniDataLookup.Update(this);


            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();

            job.collisionQuery = CollisionQuery;
            job.hpDataLookup = hpDataLookup;
            job.hitDirectionLookup = hitDirectionLookup;
            job.aniDataLookup = aniDataLookup;
            job.ecbParallel = ecbParallel;

            Dependency = job.ScheduleParallel(Dependency);
            Dependency.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        new partial struct Job : IJobEntity {

            public CollisionQuery<AgentComponent> collisionQuery;

            [ReadOnly] public ComponentLookup<HpData> hpDataLookup;
            [ReadOnly] public ComponentLookup<HitDirection> hitDirectionLookup;
            [ReadOnly] public ComponentLookup<AnimationData> aniDataLookup;
            public EntityCommandBuffer.ParallelWriter ecbParallel;

            void Execute([EntityIndexInQuery] int index, PlayerBulletTag tag,
                ref DynamicBuffer<HitedUnits> hitedUnits,in AttackData attack, in ShapeComponent shape, in LocalTransform localTrs) {
                QueryCallBack queryCallBack = new(index, in this, in hitedUnits, in attack, in localTrs);
                collisionQuery.Query(in shape, in queryCallBack);
            }

            readonly unsafe struct QueryCallBack : IQueryCallBack<AgentComponent> {

                readonly int index;
                readonly Job* job;
                readonly DynamicBuffer<HitedUnits>* hitedUnits;
                readonly AttackData* attack;
                readonly LocalTransform* localTrs;
                readonly NativeArray<Entity>.ReadOnly hitedUnitsArray;

                public QueryCallBack(int index,
                    in Job job, in DynamicBuffer<HitedUnits> hitedUnits,
                    in AttackData attack, in LocalTransform localTrs) {

                    this.index = index;
                    fixed (Job* p = &job) { this.job = p; }
                    fixed (DynamicBuffer<HitedUnits>* p = &hitedUnits) { this.hitedUnits = p; }
                    fixed (AttackData* p = &attack) { this.attack = p; }
                    fixed (LocalTransform* p = &localTrs) { this.localTrs = p; }
                    var hitedUnitsArray = new NativeArray<Entity>(hitedUnits.Length, Allocator.Temp);
                    for (int i = 0; i < hitedUnitsArray.Length; i++) {
                        hitedUnitsArray[i] = hitedUnits.ElementAt(i).Unit;
                    }
                    this.hitedUnitsArray = hitedUnitsArray.AsReadOnly();
                }

                bool IQueryCallBack<AgentComponent>.Invoke(in AgentComponent result, float distSq) {

                    bool doNext = true;


                    var hp = job->hpDataLookup[result.Entity];
                    if (hp.Hp <= 0)
                        return doNext;


                    // 伤害唯一
                    if (hitedUnitsArray.BinarySearch(result.Entity) >= 0)
                        return doNext;


                    // 造成伤害
                    hp.Hp -= attack->Attack;
                    job->ecbParallel.SetComponent(index, result.Entity, hp);


                    var hitDirection = job->hitDirectionLookup[result.Entity];
                    hitDirection.Value = localTrs->Forward().xz;
                    job->ecbParallel.SetComponent(index, result.Entity, hitDirection);


                    hitedUnits->Add(new HitedUnits { Unit = result.Entity });

                    if (job->aniDataLookup.HasComponent(result.Entity)) {
                        var ani = job->aniDataLookup[result.Entity];
                        ani.NeedState = AnimationState.Hit;
                        job->ecbParallel.SetComponent(index, result.Entity, ani);
                    }

                    return doNext;
                }
            }
        }
    }
}
