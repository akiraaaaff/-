using UnityEngine;

public class BuffRuneAttackAftershock : BuffBase
{
    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.attackAfterShockEffects += BuffUse;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.attackAfterShockEffects -= BuffUse;
    }

    private void BuffUse(Transform tarPos)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.AftershockAura);
        ts.position = tarPos.position;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: 10 + my.attack * value, damageType: DamageType.attackAftershock,
          range: DamageRange.cylinder, rangeHeight: 0.5f);
    }
}
