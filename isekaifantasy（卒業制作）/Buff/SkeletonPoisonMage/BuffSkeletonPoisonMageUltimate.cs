using System.Collections.Generic;
using UnityEngine;

public class BuffSkeletonPoisonMageUltimate : BuffBase
{
    private List<Skill> skillList = new List<Skill>();


    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.enemyDieEffects += BuffRun;
        my.friendsDieEffects += BuffRun;
    }

    protected override void BuffAddSkill()
    {
        base.BuffAddSkill();
        skillList.Add(skill);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.enemyDieEffects -= BuffRun;
        my.friendsDieEffects -= BuffRun;
    }

    public override void BuffRun(UnitBace target)
    {
        foreach (var skill in skillList)
        {
            my.SkillColdDownOne(skill, 10f);
        }
    }
}
