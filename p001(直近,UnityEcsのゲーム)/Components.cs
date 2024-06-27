using Unity.Entities;
using Unity.Mathematics;

namespace P001.SampleScene
{
    struct HpData : IComponentData
    {
        public int Hp;
        public int MaxHp;
    }
    struct AttackData : IComponentData
    {
        public int Attack;
    }



    struct NeedDestoryTag : IComponentData, IEnableableComponent {
    }
    struct TrsParent : IComponentData
    {
        public Entity Parent;
    }
    struct Height : IComponentData
    {
        public float Value;
    }
    struct ShadowSetting : IComponentData
    {
        public float Scale;
        public float Z;
    }
    struct HitDirection : IComponentData
    {
        public float2 Value;
    }
    struct ModelIndex : IComponentData
    {
        public ushort Value;
    }


    struct UnitTarget : IComponentData
    {
        public Entity Target;
    }

    [InternalBufferCapacity(8)]
    public struct HitedUnits : IBufferElementData
    {
        public Entity Unit;
    }
}
