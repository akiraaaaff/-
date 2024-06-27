using UnityEngine;

public class BuffRuneBattleRhythm : BuffBase
{
    private const float StillTime = 3;
    private float thisTimeCount;
    private bool isUse;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useSkillEffects+= OnSkill;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useSkillEffects-= OnSkill;
        if (isUse)
        {
            isUse = false;
            my.AttackSpeed -= value;
        }
        thisTimeCount = 0;
    }

    private void OnSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (!isUse)
        {
            isUse = true;
            my.AttackSpeed += value;
        }
        thisTimeCount = 0;
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        thisTimeCount += loopTime;
        if (thisTimeCount >= StillTime)
        {
            thisTimeCount = 0;
            if (isUse)
            {
                isUse = false;
                my.AttackSpeed -= value;
            }
        }
    }
}
