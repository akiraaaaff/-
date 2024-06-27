using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffRuneSacrifice : BuffBase
{
    private int nowUseTimes;
    public Vector3 position;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useSkillEffects += OnPetSkill;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useSkillEffects -= OnSkill;
        my.useSkillEffects -= OnPetSkill;
    }
    private void OnSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (skill != my.skills.itemList[0] || skill.nowUseTimes != 0 || skill.skillName == SkillName.仲間召喚)
            return;
        my.useSkillEffects -= OnSkill;
        StartCoroutine(BackCallFriends(target, skill, skillPara));
    }
    private void OnPetSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (skill.skillName != SkillName.仲間召喚 || skillPara.isDouble)
            return;
        nowUseTimes = skill.nowUseTimes;
        my.useSkillEffects -= OnPetSkill;
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.SacrificeMagic);
        position = my.trs.position;
        position.y = 0;
        ts.position = position;
        StartCoroutine(ChangeSkill(target, skill, skillPara));
    }

    private IEnumerator ChangeSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        yield return new WaitForSeconds(5f);
        var type = 1;
        if (skill.effList.Count >= 3)
            type = 2;
        PlayerManager.Instance.ResetSkillWhenHeroChange(my, SkillAni.item, 0, type);
        my.skills.itemList[0].useTimes = (int)value;
        my.skills.itemList[0].nowUseTimes = my.skills.itemList[0].useTimes;
        if (skill.effList.Count > 0)
        {
            var tempList = skill.effList.Select(x => x.GetComponent<UnitBace>());
            var unitList = new List<UnitBace>(tempList);
            foreach (var unit in unitList)
            {
                skillPara.bulletPos = unit.bulletPosMiddle;
                my.skills.itemList[0].skill?.Invoke(unit, target, my.skills.itemList[0], skillPara);
                unit.ToDeathUseEff();
            }
        }
        my.useSkillEffects += OnSkill;
    }

    private IEnumerator BackCallFriends(UnitBace target, Skill skill, SkillPara skillPara)
    {
        yield return new WaitForSeconds(GameManager.Instance.DoubleSkillWaitTime+1f);
        my.ChangSkill(PlayerManager.Instance.callFriends, SkillAni.item, index: 0);
        my.skills.itemList[0].nowUseTimes = nowUseTimes;
        my.useSkillEffects += OnPetSkill;
    }
}
