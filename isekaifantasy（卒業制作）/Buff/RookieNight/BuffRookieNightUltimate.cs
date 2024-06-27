using System.Collections.Generic;
using UnityEngine;

public class BuffRookieNightUltimate : BuffBase
{
    private ParticleSystem particle;
    private ParticleSystem.MainModule main;
    private readonly float skillCharge = 1f;
    private List<Skill> skillList = new List<Skill>();

    protected override void BuffAdd()
    {
        base.BuffAdd();
        my.beAttackEffects += BuffRun;
        my.beSkillEffects += BuffRun;
        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
            main = particle.main;
        }
        SetMaxParticles(0);
    }

    protected override void BuffAddSkill()
    {
        base.BuffAddSkill();
        skillList.Add(skill);
    }

    protected override void BuffRset()
    {
        base.BuffRset();
        foreach (var skill in skillList)
        {
            if (skill.nowChargePara < skill.chargePara && (skill.nowUseTimes > 0 || skill.useTimes == 0) && (skill.nowCoolTime == 0 || skill.coolTime == 0))
            {
                skill.nowChargePara += skillCharge;
                if (skill.nowChargePara > skill.chargePara)
                    skill.nowChargePara = skill.chargePara;
            }
        }
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.beAttackEffects -= BuffRun;
        my.beSkillEffects -= BuffRun;
        foreach (var skill in skillList)
        {
            skill.nowChargePara = 0;
        }
        skillList.Clear();
        SetMaxParticles(0);
    }

    public override void BuffSkillRemove(Skill skill)
    {
        base.BuffSkillRemove(skill);
        if (skillList.Contains(skill))
        {
            if (skillList.Count == 1)
            {
                DestroyByPool();
                return;
            }

            skillList.Remove(skill);
            skill.nowChargePara = 0;
        }
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        var charageHeight = 0f;
        foreach (var sk in skillList)
        {
            if (sk.nowChargePara == 0)
                continue;

            var para = sk.nowChargePara / sk.chargePara;
            if (para > charageHeight)
                charageHeight = para;

        }
        SetMaxParticles((int)(30 * charageHeight));
    }

    public override void BuffRun(UnitBace target)
    {
        ReSet();
    }

    private void SetMaxParticles(int max)
    {
        main.maxParticles = max;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
