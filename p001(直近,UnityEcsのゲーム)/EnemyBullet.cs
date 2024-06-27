using P001.Common;
using P001.EcsPaeticle;
using P001.Simulation.Collision;
using P001.Simulation.ORCA;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace P001.SampleScene {
    struct EnemyBulletTag : IComponentData, IEnableableComponent {

        public static void SetEnemyBullet(Entity prototype, EntityManager entityManager) {

            entityManager.AddComponentData(prototype, new EnemyBulletMoveData
            {
                MaxDistance = math.lengthsq(20f),
                Speed = 3f,
            });
            entityManager.AddComponentData(prototype, new EnemyBulletTag());
            entityManager.SetComponentEnabled<EnemyBulletTag>(prototype, false);
            entityManager.AddComponentData(prototype, new AttackData());
            entityManager.AddComponentData(prototype, new NeedDestoryTag());
            entityManager.SetComponentEnabled<NeedDestoryTag>(prototype, false);

            var shape = new ShapeComponent();
            shape.SetCircleShape(0.3f, 0, Layer.EnemyBullet, Layer.Player);
            entityManager.AddComponentData(prototype, shape);
            entityManager.SetComponentEnabled<ShapeComponent>(prototype, false);
        }
    }

    struct EnemyBulletMoveData : IComponentData {
        public float MaxDistance;
        public float Speed;
        public float2 StartPos;
        public float2 Direction;
    }

    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SampleSceneSystemGroup))]
    partial struct EnemyBulletMoveSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EnemyBulletTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();

            var job = new EnemyBulletMoveJob
            {
                deltaTime = SystemAPI.Time.fixedDeltaTime,
                ecbParallel = ecbParallel
            };
            state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency.Complete();


            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        partial struct EnemyBulletMoveJob : IJobEntity {
            public float deltaTime;
            public EntityCommandBuffer.ParallelWriter ecbParallel;

            void Execute(Entity entity, [EntityIndexInQuery] int index,
                ref LocalTransform transform, ref ShapeComponent shape, in EnemyBulletMoveData moveData) {
                if (math.distancesq(moveData.StartPos, transform.Position.xz) >= moveData.MaxDistance) {

                    ecbParallel.SetComponentEnabled<EnemyBulletTag>(index, entity, false);
                    ecbParallel.SetComponentEnabled<ParticleGroupComponent>(index, entity, false);
                    ecbParallel.SetComponentEnabled<NeedDestoryTag>(index, entity, false);
                    ecbParallel.SetComponentEnabled<ShapeComponent>(index, entity, false);
                    return;
                }

                var pos = moveData.Direction * deltaTime * moveData.Speed;
                transform.Position.xz += pos;
                shape.Position = transform.Position.xz;
            }
        }
    }


    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SampleSceneSystemGroup))]
    partial struct EnemyBulletDestorySystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EnemyBulletTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();

            var job = new EnemyBulletDestoryJob
            {
                ecbParallel = ecbParallel
            };
            state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency.Complete();


            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        partial struct EnemyBulletDestoryJob : IJobEntity {
            public EntityCommandBuffer.ParallelWriter ecbParallel;

            void Execute(Entity entity, [EntityIndexInQuery] int index,
                EnemyBulletTag tag, NeedDestoryTag needDestoryTag) {
                ecbParallel.SetComponentEnabled<EnemyBulletTag>(index, entity, false);
                ecbParallel.SetComponentEnabled<ParticleGroupComponent>(index, entity, false);
                ecbParallel.SetComponentEnabled<NeedDestoryTag>(index, entity, false);
                ecbParallel.SetComponentEnabled<ShapeComponent>(index, entity, false);
            }
        }
    }


    [DisableAutoCreation]
    [BurstCompile]
    public partial class EnemyBulletCollisionSystem : SystemBase {

        ComponentLookup<HpData> hpDataLookup;
        Job job;

        public CollisionQuery<AgentComponent> CollisionQuery;

        protected override void OnCreate() {

            hpDataLookup = GetComponentLookup<HpData>(true);
        }

        protected override void OnUpdate() {

            hpDataLookup.Update(this);


            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();

            job.collisionQuery = CollisionQuery;
            job.hpDataLookup = hpDataLookup;
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
            public EntityCommandBuffer.ParallelWriter ecbParallel;

            void Execute([EntityIndexInQuery] int index, Entity entity,
                EnemyBulletTag tag, in AttackData attack, in ShapeComponent shape) {
                QueryCallBack queryCallBack = new(index, entity, in this, in attack);
                collisionQuery.Query(in shape, in queryCallBack);
            }

            readonly unsafe struct QueryCallBack : IQueryCallBack<AgentComponent> {

                readonly int index;
                readonly Entity entity;
                readonly Job* job;
                readonly AttackData* attack;

                public QueryCallBack(int index, Entity entity, in Job job, in AttackData attack) {

                    this.index = index;
                    this.entity = entity;
                    fixed (Job* p = &job) { this.job = p; }
                    fixed (AttackData* p = &attack) { this.attack = p; }
                }

                bool IQueryCallBack<AgentComponent>.Invoke(in AgentComponent result, float distSq) {

                    bool doNext = true;


                    var hp = job->hpDataLookup[result.Entity];
                    if (hp.Hp <= 0)
                        return doNext;


                    // 造成伤害
                    hp.Hp -= attack->Attack;
                    job->ecbParallel.SetComponent(index, result.Entity, hp);

                    // 销毁子弹
                    job->ecbParallel.SetComponentEnabled<NeedDestoryTag>(index, entity, true);

                    return doNext;
                }
            }
        }
    }
}
