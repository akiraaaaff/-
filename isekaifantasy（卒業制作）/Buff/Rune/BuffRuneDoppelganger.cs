using System.Collections;
using UnityEngine;

public class BuffRuneDoppelganger : BuffBase
{
    private UnitBace ganger;
    private bool gangerIsSkill;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1)
        {
            my.inFightEffects += BuffRun;
            my.outFightEffects += GanGerOut;
            my.useSkillEffects += GangerSkill;
        }
        else
            if (buffLv == 2)
            my.useAttackEffects += GangerSkill;


        if (ganger != null)
        {
            if (buffLv == 2)
            {
                ganger.SetMaterialColorDefult(GameManager.Instance.ColorIsGangerLv2);
            }
        }
        else if (my.fightTimer > 0)
        {
            BuffRun(null);
        }
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.inFightEffects -= BuffRun;
        my.outFightEffects -= GanGerOut;
        my.useSkillEffects -= GangerSkill;
        my.useAttackEffects -= GangerSkill;
        if (ganger != null)
            ganger.ToDeathUseEff();
    }

    public override void BuffRun(UnitBace target)
    {
        if (ganger != null)
            return;
        gangerIsSkill = false;
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.SpawnMagic);
        Vector3 position = my.trs.position + Random.insideUnitSphere * 2;
        position.y = 0;
        ts.position = position;

        var prafe = PlayerManager.Instance.dicAllUnitName[my.unitName];
        ts = PoolManager.Instance.GetObj(prafe);
        ganger = ts.GetComponent<UnitBace>();
        ganger.Init();
        ganger.nav.enabled = false;
        ganger.trs.position = position;
        ganger.nav.enabled = true;
        ganger.SetGroup(GameManager.Instance.friendsList);
        ganger.Owner = my;
        ganger.isGanger = true;
        DontDestroyOnLoad(ganger);
        ganger.dieEffects += DieEixt;


        if (buffLv == 1)
            ganger.SetMaterialColorDefult(GameManager.Instance.ColorIsGangerLv1);
        else
            ganger.SetMaterialColorDefult(GameManager.Instance.ColorIsGangerLv2);
    }

    public void GanGerOut(UnitBace target)
    {
        if (ganger != null)
        {
            if (!ganger.isSkilling)
            {
                ganger.ToDeathUseEff();
            }
            else
            {
                gangerIsSkill = true;
                ganger.useAttackEffects += GanGerSkillOut;
                ganger.useSkillEffects += GanGerSkillOut;
            }
        }
    }

    public void GanGerSkillOut(UnitBace target, Skill skill, SkillPara skillPara)
    {
        StartCoroutine(IEGanGerSkillOut(target, skill, skillPara));
    }

    private IEnumerator IEGanGerSkillOut(UnitBace target, Skill skill, SkillPara skillPara)
    {
        yield return new WaitForSeconds(GameManager.Instance.DoubleSkillWaitTime + 0.3f);
        if (ganger == null)
            yield return null;
        ganger.ToDeathUseEff();
    }

    private void DieEixt(UnitBace target)
    {
        ganger.dieEffects -= DieEixt;
        if (gangerIsSkill)
        {
            gangerIsSkill = false;
            ganger.useAttackEffects -= GanGerSkillOut;
            ganger.useSkillEffects -= GanGerSkillOut;
        }
        ganger = null;
    }

    private void GangerSkill(UnitBace targer, Skill skill, SkillPara skillPara)
    {
        if (ganger == null)
            return;
        if (ganger.isSkilling)
        {
            if (skill == my.skills.attack &&
                (ganger.runningSkill.skillAni != SkillAni.attack ||
                ganger.runningSkill.skillAni != SkillAni.passive))
                return;
        }
        var tempSkill = ganger.SetSkillPara(skill,new Skill());
        tempSkill.effList = skill.effList;
        ganger.Spelld(tempSkill);
        ganger.Cold(tempSkill);
    }
}
