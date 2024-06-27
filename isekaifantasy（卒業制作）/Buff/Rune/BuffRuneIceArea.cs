using System.Collections.Generic;
using UnityEngine;

public class BuffRuneIceArea : BuffBase
{
    private List<EffectsIceArea> effList;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useSkillEffects += BuffUse;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useSkillEffects -= BuffUse;
    }

    private void BuffUse(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (target == null)
            target = my;
        if (effList == null)
            effList = new List<EffectsIceArea>();
        if (effList.Count == buffLv)
        {
            effList[0].DestroyByPool();
        }
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.IceArea);
        ts.position = target.trs.position;
        ts.rotation = my.trs.rotation;
        var eff = ts.GetComponent<EffectsIceArea>();
        effList.Add(eff);
        eff.SetList(effList);
        skillPara.power = 1;
        eff.Init(my, destroyTime: 10, attack: value + my.magic * 0.02f, damageType: DamageType.no, skillPara: skillPara, loopTime: 0.9f, buffAdd: GameManager.Instance.GameConf.runeBuff.RuneIceAreaSlow, buffTime: 1.5f, buffType: BuffType.slow, range: DamageRange.cylinder, rangeHeight: 0.5f); ;
    }
}
