using P001.Common;
using P001.EcsPaeticle;
using P001.Simulation.Collision;
using P001.Simulation.ORCA;
using P001.Simulation.Tree;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Jobs;

namespace P001.SampleScene {
    struct PlayerGenerator : IComponentData {
        public Entity ProtoType;
    }

    struct PlayerTag : IComponentData {
    }

    struct AttactVelocity : IComponentData {
        public float2 Value;
    }


    [DisableAutoCreation]
    [BurstCompile]
    partial struct PlayerGenerateSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var generator = SystemAPI.GetSingleton<PlayerGenerator>();

            state.EntityManager.AddComponentData(generator.ProtoType, new PlayerTag());
            state.EntityManager.AddComponentData(generator.ProtoType, new Height { Value = 1.3f });
            state.EntityManager.AddComponentData(generator.ProtoType, new ShadowSetting { Scale = 0.9f, Z = 0f });


            var hp = 500;
            state.EntityManager.AddComponentData(generator.ProtoType, new HpData { Hp = hp, MaxHp = hp });
            state.EntityManager.AddComponentData(generator.ProtoType, new AttackData { Attack = 150 });
            state.EntityManager.AddComponentData(generator.ProtoType, new UnitTarget());
            state.EntityManager.AddComponentData(generator.ProtoType, new AttactVelocity());



            state.EntityManager.AddComponentData(generator.ProtoType, new AgentComponent
            {
                MaxSpeed = 3.0f,
                LayerOccupation = Layer.Player,
                Blob = AgentBlob.CreateReference(0.3f, byte.MaxValue, Layer.Player | Layer.Enemy | Layer.EnemyBullet),
            });



            var newPlayer = state.EntityManager.Instantiate(generator.ProtoType);

            state.EntityManager.SetComponentEnabled<AnimationData>(newPlayer, true);
            var animationData = state.EntityManager.GetComponentData<AnimationData>(newPlayer);
            var matMeshInfo = state.EntityManager.GetComponentData<MaterialMeshInfo>(newPlayer);
            matMeshInfo.MeshID = animationData.Blob.Value.MeshID;
            state.EntityManager.SetComponentData(newPlayer, matMeshInfo);

            var agentComponent = state.EntityManager.GetComponentData<AgentComponent>(newPlayer);
            agentComponent.Entity = newPlayer;
            agentComponent.Velocity = new float2(0, -1f);
            state.EntityManager.SetComponentData(newPlayer, agentComponent);

            var attactVelocity = state.EntityManager.GetComponentData<AttactVelocity>(newPlayer);
            attactVelocity.Value = new float2(0, -1f);
            state.EntityManager.SetComponentData(newPlayer, attactVelocity);
        }
    }


    [BurstCompile]
    [DisableAutoCreation]
    public partial class PlayerTreeBuildSystem : SystemBase {

        EntityQuery agentQuery;
        NativeArray<AgentComponent> agents;

        KDTreeBuildJob<AgentComponent> agentKDTreeBuildJob;
        NativeList<TreeNode> agentTree;

        public CollisionQuery<AgentComponent> CollisionQuery;

        [BurstCompile]
        protected override void OnCreate() {

            var agentQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<AgentComponent, PlayerTag>();
            agentQuery = GetEntityQuery(agentQueryBuilder);

            agentTree = new NativeList<TreeNode>(Allocator.Persistent);
        }

        [BurstCompile]
        protected override void OnDestroy() {

            agentTree.Dispose();

            if (agents.IsCreated) {
                agents.Dispose();
            }
        }

        [BurstCompile]
        protected override void OnUpdate() {

            if (agents.IsCreated) {
                agents.Dispose();
            }
            agents = agentQuery.ToComponentDataArray<AgentComponent>(Allocator.TempJob);


            agentKDTreeBuildJob.outputComponents = agents;
            agentKDTreeBuildJob.outputTree = agentTree;
            Dependency = agentKDTreeBuildJob.Schedule(Dependency);
            Dependency.Complete();

            CollisionQuery.inputComponents = agents.AsReadOnly();
            CollisionQuery.inputTree = agentTree.AsReadOnly();
        }
    }



    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SampleSceneSystemGroup))]
    partial struct PlayerMoveSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var joystickData = SystemAPI.GetSingleton<JoystickData>();
            foreach (var (agentComponent, aniData, tag) in SystemAPI.Query<RefRW<AgentComponent>, RefRW<AnimationData>, PlayerTag>()) {

                if (math.any(joystickData.Input != 0)) {
                    var direction = math.forward() * joystickData.Input.y + math.right() * joystickData.Input.x;
                    agentComponent.ValueRW.TargetPosition = agentComponent.ValueRW.Position + direction.xz * agentComponent.ValueRW.Blob.Value.Radius;
                    aniData.ValueRW.NeedState = AnimationState.Run;
                }
                else {
                    agentComponent.ValueRW.TargetPosition = agentComponent.ValueRW.Position;
                    aniData.ValueRW.NeedState = AnimationState.Idle;
                }
            }
        }
    }



    [BurstCompile]
    [DisableAutoCreation]
    partial struct PlayerAttackLookAtSystem : ISystem {

        Job job;
        ComponentLookup<AgentComponent> agentLookup;
        EntityStorageInfoLookup entityLookup;


        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            agentLookup = state.GetComponentLookup<AgentComponent>(true);
            entityLookup = state.GetEntityStorageInfoLookup();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {

            agentLookup.Update(ref state);
            entityLookup.Update(ref state);

            job.agentLookup = agentLookup;
            job.entityLookup = entityLookup;
            state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency.Complete();
        }

        [BurstCompile]
        partial struct Job : IJobEntity {
            [ReadOnly] public ComponentLookup<AgentComponent> agentLookup;
            [ReadOnly] public EntityStorageInfoLookup entityLookup;

            void Execute(Entity entity, PlayerTag tag, in UnitTarget unitTarget, ref AttactVelocity attactVelocity) {

                if (!entityLookup.Exists(unitTarget.Target))
                    return;

                attactVelocity.Value = agentLookup.GetRefRO(unitTarget.Target).ValueRO.Position - agentLookup.GetRefRO(entity).ValueRO.Position;
            }
        }
    }

    [BurstCompile]
    [DisableAutoCreation]
    public partial struct PlayerTransformWriteSystem : ISystem {

        Job job;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency.Complete();
        }

        [BurstCompile]
        partial struct Job : IJobEntity {

            void Execute(ref LocalTransform localTrs, in AgentComponent agent, in AttactVelocity attactVelocity) {
                localTrs.Position.xz = agent.Position;

                if (math.any(attactVelocity.Value != 0)) {
                    // 弧度1.047198f=角度60
                    var radFixed = 1.22f;
                    // 伪2D视角
                    var radY = math.atan2(attactVelocity.Value.x, attactVelocity.Value.y);
                    var radX = math.cos(radY) * radFixed;
                    var radZ = math.sin(radY) * radFixed;

                    localTrs.Rotation = quaternion.EulerZXY(radX, radY, radZ);
                }
            }
        }
    }



    [DisableAutoCreation]
    [BurstCompile]
    partial class PlayerBehaviourSystem : SystemBase {

        ComponentLookup<EnemyTag> enemyTagLookup;
        ComponentLookup<LocalToWorld> localToWorldLookup;

        EntityQuery particleGroupQuery;
        NativeList<Entity> particleGroupList;

        public CollisionQuery<AgentComponent> CollisionQuery;

        protected override void OnCreate() {
            RequireForUpdate<ParticleGroupComponent>();
            enemyTagLookup = GetComponentLookup<EnemyTag>(true);
            localToWorldLookup = GetComponentLookup<LocalToWorld>(true);

            var particleGroupQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<ParticleGroupComponent, PlayerBulletTag>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            particleGroupQuery = GetEntityQuery(particleGroupQueryBuilder);
            particleGroupList = new NativeList<Entity>(Allocator.Persistent);
        }

        protected override void OnDestroy() {
            particleGroupList.Dispose();
        }

        protected override void OnUpdate() {

            this.enemyTagLookup.Update(this);
            this.localToWorldLookup.Update(this);
            var enemyTagLookup = this.enemyTagLookup;
            var localToWorldLookup = this.localToWorldLookup;
            var collisionQuery = this.CollisionQuery;


            // 寻找最近的敌人
            Entities
                .WithBurst()
                .WithReadOnly(collisionQuery)
                .ForEach((Entity entity, PlayerTag tag, ref UnitTarget target, in AgentComponent agent) => {

                    if (collisionQuery.QueryClosest(agent.Position, 10f, Layer.Enemy, out var result)) {
                        target.Target = result.Entity;
                    }
                })
                .ScheduleParallel();


            // 寻找可用子弹
            particleGroupList.Clear();
            var particleGroups = particleGroupQuery.ToEntityArray(Allocator.Temp);
            foreach (var particleGroup in particleGroups) {

                var isParticleGroupEnabled = EntityManager.IsComponentEnabled<ParticleGroupComponent>(particleGroup);
                if (!isParticleGroupEnabled) {
                    particleGroupList.Add(particleGroup);
                }
            }
            if (particleGroupList.Length <= 0) {
                var prefabEntityData = SystemAPI.GetSingleton<PrefabEntityData>();
                var instantiateParticleGroups = ParticleGroupComponent.InstantiateParticleGroup(prefabEntityData.PlayerBullet, 10, EntityManager);
                particleGroupList.AddRange(instantiateParticleGroups);
            }


            // 发射子弹
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();
            var tempParticleGroupList = particleGroupList;
            var tempParticleGroup = tempParticleGroupList[0];
            var protoTypeTrs = SystemAPI.GetComponent<LocalTransform>(tempParticleGroup);
            var protoTypeMove = SystemAPI.GetComponent<PlayerBulletMoveData>(tempParticleGroup);
            Dependency = Entities
                .WithBurst()
                .WithReadOnly(tempParticleGroupList)
                .WithReadOnly(enemyTagLookup)
                .WithReadOnly(localToWorldLookup)
                .ForEach((int entityInQueryIndex, PlayerTag tag, ref UnitTarget target, in LocalTransform trs, in AttackData attackData) => {
                    if (!localToWorldLookup.HasComponent(target.Target))
                        return;
                    if (!enemyTagLookup.IsComponentEnabled(target.Target))
                        return;

                    var targetPos = localToWorldLookup[target.Target].Position;
                    var direction = targetPos - trs.Position;
                    var rotation = quaternion.LookRotation(direction, math.up());

                    var newBullet = tempParticleGroupList[entityInQueryIndex];
                    var newBulletTrs = protoTypeTrs;
                    newBulletTrs.Position.xz = trs.Position.xz + math.normalize(direction.xz) * 0.8f;
                    newBulletTrs.Position.z += 0.5f;
                    newBulletTrs.Position.y = 1.2f;
                    newBulletTrs.Rotation = rotation;
                    ecbParallel.SetComponent(entityInQueryIndex, newBullet, newBulletTrs);
                    var newBulletMove = protoTypeMove;
                    newBulletMove.StartPos = newBulletTrs.Position.xz;
                    ecbParallel.SetComponent(entityInQueryIndex, newBullet, newBulletMove);
                    var newBulletAttack = attackData;
                    ecbParallel.SetComponent(entityInQueryIndex, newBullet, newBulletAttack);

                    ecbParallel.SetComponentEnabled<ParticleGroupComponent>(newBullet.Index, newBullet, true);
                    ecbParallel.SetComponentEnabled<PlayerBulletTag>(newBullet.Index, newBullet, true);
                    ecbParallel.SetComponentEnabled<ShapeComponent>(newBullet.Index, newBullet, true);
                })
                .ScheduleParallel(Dependency);

            Dependency.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}
