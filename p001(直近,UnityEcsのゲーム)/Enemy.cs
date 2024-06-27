using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using P001.EcsPaeticle;
using Unity.Rendering;
using P001.Simulation.ORCA;
using P001.Common;
using P001.Simulation.Collision;
using P001.Simulation.Tree;
using Unity.Jobs;

namespace P001.SampleScene {
    struct EnemyGenerator : IComponentData {
        public Entity ProtoType1;
        public int Count1;
        public Entity ProtoType2;
        public int Count2;
        public Entity ProtoType3;
        public int Count3;
    }
    struct EnemyTag : IComponentData, IEnableableComponent {
    }
    struct CanEmissionTag : IComponentData {
    }


    [DisableAutoCreation]
    [BurstCompile]
    partial struct EnemyInitSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var generator = SystemAPI.GetSingleton<EnemyGenerator>();


            // 卵虫
            var height1 = new Height { Value = 0.5f };
            var shadowSetting1 = new ShadowSetting { Scale = 0.8f, Z = 0f };
            InitProto(ref state, generator.ProtoType1, height1, shadowSetting1, 50, 12, 0.5f, 0, 1);
            state.EntityManager.AddComponentData(generator.ProtoType1, new Tag3D());

            // 史莱姆
            var height2 = new Height { Value = 0.8f };
            var shadowSetting2 = new ShadowSetting { Scale = 1.2f, Z = 0.2f };
            InitProto(ref state, generator.ProtoType2, height2, shadowSetting2, 25, 5, 0.5f, 2, 0);
            state.EntityManager.AddComponentData(generator.ProtoType2, new Tag2D());

            // 喷子虫
            var height3 = new Height { Value = 1.5f };
            var shadowSetting3 = new ShadowSetting { Scale = 1f, Z = -0.2f };
            InitProto(ref state, generator.ProtoType3, height3, shadowSetting3, 250, 100, 1.2f, 1, 2);
            state.EntityManager.AddComponentData(generator.ProtoType3, new CanEmissionTag());
            state.EntityManager.AddComponentData(generator.ProtoType3, new Tag3D());
        }

        [BurstCompile]
        private void InitProto(ref SystemState state, Entity proto,
             Height height, ShadowSetting shadowSetting, int hp, int attack, float radius, ushort modelIndex, byte order) {

            state.EntityManager.AddComponentData(proto, new EnemyTag());
            state.EntityManager.SetComponentEnabled<EnemyTag>(proto, false);
            state.EntityManager.AddComponentData(proto, height);
            state.EntityManager.AddComponentData(proto, shadowSetting);

            state.EntityManager.AddComponentData(proto, new ModelIndex { Value = modelIndex });
            state.EntityManager.AddComponentData(proto, new HitDirection());


            state.EntityManager.AddComponentData(proto, new HpData { Hp = hp, MaxHp = hp });
            state.EntityManager.AddComponentData(proto, new AttackData { Attack = attack });
            state.EntityManager.AddComponentData(proto, new UnitTarget());


            state.EntityManager.AddComponentData(proto, new AgentComponent
            {
                MaxSpeed = 1.5f,
                LayerOccupation = Layer.Enemy,
                Blob = AgentBlob.CreateReference(radius, order, Layer.Player | Layer.Enemy | Layer.PlayerBullet),
            });
            state.EntityManager.SetComponentEnabled<AgentComponent>(proto, false);
        }
    }


    [DisableAutoCreation]
    [BurstCompile]
    partial class EnemyGeneratorSystem : SystemBase {

        NativeList<Entity> enemyList;
        EntityQuery enemyQuery;
        EntityQuery barQuery;
        EntityQuery shadowQuery;

        public CollisionQuery<AgentComponent> CollisionQuery;

        [BurstCompile]
        protected override void OnCreate() {

            enemyList = new NativeList<Entity>(Allocator.Persistent);

            var enemyQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<EnemyTag>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            enemyQuery = GetEntityQuery(enemyQueryBuilder);

            var barQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<EnemyHpBarTag>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            barQuery = GetEntityQuery(barQueryBuilder);

            var shadowQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<ShadowTag>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            shadowQuery = GetEntityQuery(shadowQueryBuilder);
        }

        [BurstCompile]
        protected override void OnDestroy() {
            enemyList.Dispose();
        }
        protected override void OnUpdate() {
        }

        [BurstCompile]
        public void DoInstantiate() {

            var enemyGenerator = SystemAPI.GetSingleton<EnemyGenerator>();
            // 生成敌人
            var enemy1s = CollectionHelper.CreateNativeArray<Entity>(1000, Allocator.Temp);
            EntityManager.Instantiate(enemyGenerator.ProtoType1, enemy1s);
            var enemy2s = CollectionHelper.CreateNativeArray<Entity>(5000, Allocator.Temp);
            EntityManager.Instantiate(enemyGenerator.ProtoType2, enemy2s);
            var enemy3s = CollectionHelper.CreateNativeArray<Entity>(400, Allocator.Temp);
            EntityManager.Instantiate(enemyGenerator.ProtoType3, enemy3s);
            var enemies = new NativeList<Entity>(enemy1s.Length + enemy2s.Length + enemy3s.Length, Allocator.Temp);
            enemies.AddRange(enemy1s);
            enemies.AddRange(enemy2s);
            enemies.AddRange(enemy3s);

            var barGenerator = SystemAPI.GetSingleton<EnemyHpBarGenerator>();
            var shadowGenerator = SystemAPI.GetSingleton<ShadowGenerator>();

            // 生成血条
            var bars = CollectionHelper.CreateNativeArray<Entity>(enemies.Length, Allocator.Temp);
            EntityManager.Instantiate(barGenerator.ProtoType, bars);
            // 生成影子
            var shadows = CollectionHelper.CreateNativeArray<Entity>(enemies.Length, Allocator.Temp);
            EntityManager.Instantiate(shadowGenerator.ProtoType, shadows);


            foreach (var enemy in enemies) {
                var agentComponent = EntityManager.GetComponentData<AgentComponent>(enemy);
                agentComponent.Entity = enemy;
                EntityManager.SetComponentData(enemy, agentComponent);
            }
        }

        [BurstCompile]
        public void DoSpawn(int index, int needCount) {

            enemyList.Clear();

            var enemies = enemyQuery.ToEntityArray(Allocator.Temp);
            var queryCount = 0;
            foreach (var enemy in enemies) {

                if (queryCount >= needCount)
                    continue;
                if (EntityManager.IsComponentEnabled<EnemyTag>(enemy))
                    continue;
                if (EntityManager.GetComponentData<ModelIndex>(enemy).Value != index)
                    continue;

                enemyList.Add(enemy);

                // 设置敌人
                EntityManager.SetComponentEnabled<EnemyTag>(enemy, true);
                var random = SystemAPI.GetSingletonRW<RandomSingleton>();

                var pos = random.ValueRW.Random.NextFloat2Direction() * random.ValueRW.Random.NextFloat(0f, 15f);
                if (CollisionQuery.QueryClosest(pos, 2f, Layer.Player, out var result)) {
                    pos += math.normalize(pos - result.Position) * 2f;
                }


                var localTrs = EntityManager.GetComponentData<LocalTransform>(enemy);
                localTrs.Position.xz = pos;
                EntityManager.SetComponentData(enemy, localTrs);

                EntityManager.SetComponentEnabled<AnimationData>(enemy, true);
                var animationData = EntityManager.GetComponentData<AnimationData>(enemy);
                var matMeshInfo = EntityManager.GetComponentData<MaterialMeshInfo>(enemy);
                matMeshInfo.MeshID = animationData.Blob.Value.MeshID;
                EntityManager.SetComponentData(enemy, matMeshInfo);

                var hpData = EntityManager.GetComponentData<HpData>(enemy);
                hpData.Hp = hpData.MaxHp;
                EntityManager.AddComponentData(enemy, hpData);

                EntityManager.SetComponentEnabled<AgentComponent>(enemy, true);
                var agentComponent = EntityManager.GetComponentData<AgentComponent>(enemy);
                agentComponent.Position = pos;
                EntityManager.SetComponentData(enemy, agentComponent);

                queryCount++;
            }

            if (enemyList.Length <= 0) { return; }



            var bars = barQuery.ToEntityArray(Allocator.Temp);
            var enemyIndexInBar = 0;
            foreach (var bar in bars) {

                if (EntityManager.IsComponentEnabled<HpBarTag>(bar)) { continue; }

                EntityManager.SetComponentEnabled<HpBarTag>(bar, true);

                var enemy = enemyList[enemyIndexInBar];
                // 设置血条
                var hp = EntityManager.GetComponentData<HpData>(enemy).MaxHp;
                EntityManager.SetComponentData(bar, new TrsParent { Parent = enemy, });
                EntityManager.SetComponentData(bar, new MtBarHp { Value = hp, });
                EntityManager.SetComponentData(bar, new MtBarCache { Value = hp, });
                EntityManager.SetComponentData(bar, new MtBarMaxHp { Value = hp, });

                var modelComponent = EntityManager.GetComponentData<ModelComponent>(bar);
                var matMeshInfo = EntityManager.GetComponentData<MaterialMeshInfo>(bar);
                matMeshInfo.MeshID = modelComponent.Blob.Value.MeshID;
                EntityManager.SetComponentData(bar, matMeshInfo);

                if (enemyIndexInBar >= enemyList.Length - 1) { break; }
                enemyIndexInBar++;
            }

            var shadows = shadowQuery.ToEntityArray(Allocator.Temp);
            var enemyIndexInShadow = 0;
            foreach (var shadow in shadows) {

                if (EntityManager.IsComponentEnabled<ShadowTag>(shadow)) { continue; }

                EntityManager.SetComponentEnabled<ShadowTag>(shadow, true);

                var enemy = enemyList[enemyIndexInShadow];
                // 设置影子
                var scale = EntityManager.GetComponentData<ShadowSetting>(enemy).Scale;
                EntityManager.SetComponentData(shadow, new TrsParent { Parent = enemy, });
                var trs = EntityManager.GetComponentData<LocalToWorld>(shadow);
                trs.Value.c0.x = trs.Value.c1.y = trs.Value.c2.z = scale;
                EntityManager.SetComponentData(shadow, trs);

                var modelComponent = EntityManager.GetComponentData<ModelComponent>(shadow);
                var matMeshInfo = EntityManager.GetComponentData<MaterialMeshInfo>(shadow);
                matMeshInfo.MeshID = modelComponent.Blob.Value.MeshID;
                EntityManager.SetComponentData(shadow, matMeshInfo);

                if (enemyIndexInShadow >= enemyList.Length - 1) { break; }
                enemyIndexInShadow++;
            }
        }
    }


    [BurstCompile]
    [DisableAutoCreation]
    public partial class EnemyTreeBuildSystem : SystemBase {

        EntityQuery agentQuery;
        NativeArray<AgentComponent> agents;

        KDTreeBuildJob<AgentComponent> agentKDTreeBuildJob;
        NativeList<TreeNode> agentTree;

        public CollisionQuery<AgentComponent> CollisionQuery;

        [BurstCompile]
        protected override void OnCreate() {

            var agentQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<AgentComponent, EnemyTag>();
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
    partial struct EnemyTargetTrackSystem : ISystem {

        Job job;
        ComponentLookup<AgentComponent> agentLookup;
        EntityStorageInfoLookup entityLookup;


        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EnemyGenerator>();
            state.RequireForUpdate<PlayerGenerator>();
            agentLookup = state.GetComponentLookup<AgentComponent>(false);
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
            [NativeDisableParallelForRestriction] public ComponentLookup<AgentComponent> agentLookup;
            [ReadOnly] public EntityStorageInfoLookup entityLookup;

            void Execute(Entity entity, EnemyTag tag, in UnitTarget unitTarget) {

                if (!entityLookup.Exists(unitTarget.Target))
                    return;

                agentLookup.GetRefRW(entity).ValueRW.TargetPosition = agentLookup.GetRefRO(unitTarget.Target).ValueRO.Position;
            }
        }
    }



    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(TransformSystemGroup))]
    [UpdateBefore(typeof(LocalToWorldSystem))]
    partial struct EnemyDestorySystem : ISystem {
        EntityQuery enemyQuery;


        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EnemyTag>();
            var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<HpData, EnemyTag>();
            enemyQuery = state.GetEntityQuery(queryBuilder);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var shaderTime = (float)SystemAPI.Time.ElapsedTime;

            var breakGenerator = SystemAPI.GetSingleton<BreakerGenerator>();
            var slimeDieGenerator = SystemAPI.GetSingleton<DieGenerator>();
            var r = SystemAPI.GetSingleton<RandomSingleton>();

            var maxRadians = new float3(3.141593f);
            var minRadians = -maxRadians;


            // 获取所有符合条件的实体
            var enemys = enemyQuery.ToEntityArray(Allocator.Temp);

            foreach (var enemy in enemys) {

                var hp = state.EntityManager.GetComponentData<HpData>(enemy);
                var position = state.EntityManager.GetComponentData<LocalTransform>(enemy).Position;
                var modelIndex = state.EntityManager.GetComponentData<ModelIndex>(enemy);


                // 史莱姆
                if (modelIndex.Value >= 2u) {
                    if (hp.Hp <= 0) {
                        var dier = state.EntityManager.Instantiate(slimeDieGenerator.ProtoType);
                        var localTrs = state.EntityManager.GetComponentData<LocalTransform>(dier);
                        localTrs.Position = position;
                        state.EntityManager.SetComponentData(dier, localTrs);
                        state.EntityManager.SetComponentData(dier, new DieData
                        {
                            Time = 1.14f,
                        });
                        state.EntityManager.SetComponentData(dier, new MatDeltaTimeData
                        {
                            Value = shaderTime,
                        });

                        state.EntityManager.SetComponentEnabled<EnemyTag>(enemy, false);
                        state.EntityManager.SetComponentEnabled<AnimationData>(enemy, false);
                        state.EntityManager.SetComponentEnabled<AgentComponent>(enemy, false);
                        var matMeshInfo = state.EntityManager.GetComponentData<MaterialMeshInfo>(enemy);
                        matMeshInfo.Mesh = 0;
                        state.EntityManager.SetComponentData(enemy, matMeshInfo);
                    }
                    continue;
                }


                // 3D敌人
                var protoTypes = modelIndex.Value == 0u ? breakGenerator.ProtoType0 : breakGenerator.ProtoType1;
                var enemyTrs = state.EntityManager.GetComponentData<LocalTransform>(enemy);


                if (hp.Hp <= 0) {
                    var hitDirection = state.EntityManager.GetComponentData<HitDirection>(enemy).Value;


                    foreach (var protoType in protoTypes) {
                        var breaker = state.EntityManager.Instantiate(protoType);
                        var localTrs = state.EntityManager.GetComponentData<LocalTransform>(breaker);
                        localTrs.Rotation = enemyTrs.Rotation;
                        localTrs.Position = position + math.mul(localTrs.Rotation, localTrs.Position);
                        state.EntityManager.SetComponentData(breaker, localTrs);


                        var velocity = new float3(hitDirection.x * 10, 0, hitDirection.y * 10);
                        var minVelocity = velocity - 1f;
                        var maxVelocity = velocity + 1f;
                        var minAnti = new float3(hitDirection.x * 0.5f, 0, hitDirection.y * 0.5f);
                        var maxAnti = new float3(hitDirection.x * 1.5f, 2, hitDirection.y * 1.5f);

                        state.EntityManager.SetComponentData(breaker, new BreakerData
                        {
                            Anti = r.Random.NextFloat3(minAnti, maxAnti),
                            Velocity = r.Random.NextFloat3(minVelocity, maxVelocity),
                            Quaternion = quaternion.Euler(r.Random.NextFloat3(minRadians, maxRadians) * 0.04f),
                        });
                    }

                    state.EntityManager.SetComponentEnabled<EnemyTag>(enemy, false);
                    state.EntityManager.SetComponentEnabled<AnimationData>(enemy, false);
                    state.EntityManager.SetComponentEnabled<AgentComponent>(enemy, false);
                    var matMeshInfo = state.EntityManager.GetComponentData<MaterialMeshInfo>(enemy);
                    matMeshInfo.Mesh = 0;
                    state.EntityManager.SetComponentData(enemy, matMeshInfo);
                }
            }


            SystemAPI.SetSingleton(r);
        }
    }



    [DisableAutoCreation]
    [BurstCompile]
    public partial class EnemyBehaviourSystem : SystemBase {
        ComponentLookup<LocalToWorld> localToWorldLookup;
        EntityStorageInfoLookup entityLookup;

        EntityQuery particleGroupQuery;
        NativeList<Entity> particleGroupList;

        public CollisionQuery<AgentComponent> CollisionQuery;

        protected override void OnCreate() {
            RequireForUpdate<ParticleGroupComponent>();
            localToWorldLookup = GetComponentLookup<LocalToWorld>(true);
            entityLookup = GetEntityStorageInfoLookup();

            var particleGroupQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<ParticleGroupComponent, EnemyBulletTag>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            particleGroupQuery = GetEntityQuery(particleGroupQueryBuilder);
            particleGroupList = new NativeList<Entity>(Allocator.Persistent);
        }

        protected override void OnDestroy() {
            particleGroupList.Dispose();
        }

        protected override void OnUpdate() {
            this.localToWorldLookup.Update(this);
            this.entityLookup.Update(this);
            var localToWorldLookup = this.localToWorldLookup;
            var entityLookup = this.entityLookup;
            var collisionQuery = this.CollisionQuery;


            // 寻找最近的玩家单位
            Entities
                .WithBurst()
                .WithReadOnly(entityLookup)
                .WithReadOnly(collisionQuery)
                .ForEach((Entity entity, EnemyTag tag, ref UnitTarget target, in AgentComponent agent) => {

                    if (entityLookup.Exists(target.Target))
                        return;

                    if (collisionQuery.QueryClosest(agent.Position, 9999f, Layer.Player, out var result)) {
                        target.Target = result.Entity;
                    }
                })
                .ScheduleParallel();


            // 寻找可用子弹
            particleGroupList.Clear();
            var particleGroups = particleGroupQuery.ToEntityArray(Allocator.Temp);
            foreach (var particleGroup in particleGroups) {

                if (EntityManager.IsComponentEnabled<ParticleGroupComponent>(particleGroup))
                    continue;
                particleGroupList.Add(particleGroup);
            }
            if (particleGroupList.Length <= 0) {
                var prefabEntityData = SystemAPI.GetSingleton<PrefabEntityData>();
                var instantiateParticleGroups = ParticleGroupComponent.InstantiateParticleGroup(prefabEntityData.EnemyBullet, 100, EntityManager);
                particleGroupList.AddRange(instantiateParticleGroups);
            }


            // 发射子弹
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();
            var tempParticleGroupList = particleGroupList;
            var tempParticleGroup = tempParticleGroupList[0];
            var protoTypeTrs = SystemAPI.GetComponent<LocalTransform>(tempParticleGroup);
            var protoTypeMove = SystemAPI.GetComponent<EnemyBulletMoveData>(tempParticleGroup);
            Dependency = Entities
                .WithBurst()
                .WithReadOnly(localToWorldLookup)
                .WithReadOnly(tempParticleGroupList)
                .WithReadOnly(entityLookup)
                .ForEach((int entityInQueryIndex, EnemyTag tag, CanEmissionTag canEmissionTag, ref UnitTarget target,
                in LocalTransform trs, in AttackData attackData) => {
                    if (!entityLookup.Exists(target.Target))
                        return;
                    if (!localToWorldLookup.HasComponent(target.Target))
                        return;
                    if (entityInQueryIndex >= tempParticleGroupList.Length)
                        return;

                    var targetPos = localToWorldLookup[target.Target].Position;
                    var direction = math.normalize(targetPos - trs.Position);
                    var radY = math.atan2(direction.x, direction.z);
                    // 弧度1.570796f=角度90
                    var rotation = quaternion.EulerZXY(1.570796f, radY, 0);

                    var newBullet = tempParticleGroupList[entityInQueryIndex];
                    var newBulletTrs = protoTypeTrs;
                    newBulletTrs.Position.xz = trs.Position.xz;
                    newBulletTrs.Position.z += 0.4f;
                    newBulletTrs.Rotation = rotation;
                    ecbParallel.SetComponent(entityInQueryIndex, newBullet, newBulletTrs);
                    var newBulletMove = protoTypeMove;
                    newBulletMove.StartPos = newBulletTrs.Position.xz;
                    newBulletMove.Direction = direction.xz;
                    ecbParallel.SetComponent(entityInQueryIndex, newBullet, newBulletMove);
                    var newBulletAttack = attackData;
                    ecbParallel.SetComponent(entityInQueryIndex, newBullet, newBulletAttack);

                    ecbParallel.SetComponentEnabled<ParticleGroupComponent>(entityInQueryIndex, newBullet, true);
                    ecbParallel.SetComponentEnabled<EnemyBulletTag>(entityInQueryIndex, newBullet, true);
                    ecbParallel.SetComponentEnabled<ShapeComponent>(entityInQueryIndex, newBullet, true);
                })
                .ScheduleParallel(Dependency);

            Dependency.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();

        }
    }


    [DisableAutoCreation]
    [BurstCompile]
    public partial class EnemyCollisionSystem : SystemBase {

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
                EnemyTag tag, in AttackData attack, in AgentComponent agent) {
                QueryCallBack queryCallBack = new(index, entity, in this, in attack);
                collisionQuery.Query(in agent, in queryCallBack);
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

                    return doNext;
                }
            }
        }
    }
}
