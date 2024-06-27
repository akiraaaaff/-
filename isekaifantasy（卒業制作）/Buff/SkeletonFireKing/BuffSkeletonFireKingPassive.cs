using UnityEngine;

public class BuffSkeletonFireKingPassive : BuffBase
{

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.friendsDieEffects += BuffRun;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.friendsDieEffects -= BuffRun;
    }

    public override void BuffRun(UnitBace target)
    {
        my.TakeDamage(30, my, DamageType.heal);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.FireFieldRed);
        ts.position = target.trs.position;
        ts.rotation = target.trs.rotation;
        EffectsBase eff2 = ts.GetComponent<EffectsBase>();
        eff2.Init(my, destroyTime: 10, attack: 10 , damageType: DamageType.no, range: DamageRange.hemisphere, loopTime: 0.5f);
    }
}
