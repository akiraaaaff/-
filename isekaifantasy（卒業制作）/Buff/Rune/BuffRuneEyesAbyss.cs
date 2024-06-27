using UnityEngine;

public class BuffRuneEyesAbyss : BuffBase
{
    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.targetChangeEffects += BuffRun;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.targetChangeEffects -= BuffRun;
    }

    public override void BuffRun(UnitBace target)
    {
        if (target.CheckBuff(GameManager.Instance.GameConf.runeBuff.RuneEyesTime.name))
            return;
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.DarkBlast);
        ts.position = target.trs.position;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, buffAdd: GameManager.Instance.GameConf.runeBuff.RuneEyesTime, buffTime: 5f, buffType: BuffType.plus, attack: value + my.magic * 0.4f + target.Hp * 0.1f, damageType: DamageType.magic, clip: GameManager.Instance.GameConf.sounds.MagicBurst,
            reSetDamage: GetDamage);
    }
    private ReSetDamage GetDamage(UnitBace attacker, UnitBace target)
    {
        var data = new ReSetDamage();
        if (target.CheckBuff(GameManager.Instance.GameConf.runeBuff.RuneEyesTime.name))
            return data;

        data.isDamage = true;
        data.damageType = DamageType.magic;
        if (target == null)
            data.damage= value + attacker.magic * 0.1f;
        else
            data.damage= value + attacker.magic * 0.1f + target.Hp * 0.2f;

        return data;
    }
}
