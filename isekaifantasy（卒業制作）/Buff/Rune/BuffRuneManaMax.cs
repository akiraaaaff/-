using UnityEngine;

public class BuffRuneManaMax : BuffBase
{
    private int allValue;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.attackEffects += BuffRun;
        my.useSkillEffects+= OnSkill;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.attackEffects -= BuffRun;
        my.useSkillEffects-= OnSkill;
    }

    public override void BuffRun(UnitBace target)
    {
        var ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.MpRestore);
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;

        var mpPlus = 1;
        if (value - allValue < mpPlus)
            mpPlus = (int)value - allValue;
        my.Mp += mpPlus;
        my.mpMax += mpPlus;
        allValue += mpPlus;
        if (allValue >= value)
        {
            my.attackEffects -= BuffRun;
            my.useSkillEffects -= OnSkill;
        }
    }

    private void OnSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        var ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.MpRestore);
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;

        var mpPlus = 10;
        if (value - allValue < mpPlus)
            mpPlus = (int)value - allValue;
        my.Mp += mpPlus;
        my.mpMax += mpPlus;
        allValue += mpPlus;
        if (allValue >= value)
        {
            my.attackEffects -= BuffRun;
            my.useSkillEffects -= OnSkill;
        }
    }
}
