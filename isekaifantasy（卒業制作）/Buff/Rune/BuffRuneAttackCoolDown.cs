using UnityEngine;

public class BuffRuneAttackCoolDown : BuffBase
{
    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useAttackEffects += BuffUse;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useAttackEffects -= BuffUse;
    }

    private void BuffUse(UnitBace target, Skill skill, SkillPara skillPara)
    {
        my.SkillColdDownAll();
    }
}
