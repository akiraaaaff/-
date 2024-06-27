using UnityEngine;
using System.Collections;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、Eggyのスキルを制御する。
/// </summary>
public class CougarCub : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.斬撃,
            spell = SpellType.click,
            aiType = AiType.chase,
            max = 5,
        };
        skills.attack.skill = MyAttack;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        CommonSkillsManager.Instance.AttackNormal(my, target, skill, skillPara);
    }
}
