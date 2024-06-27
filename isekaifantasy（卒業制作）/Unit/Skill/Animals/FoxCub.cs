using UnityEngine;
using System.Collections;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、Eggyのスキルを制御する。
/// </summary>
public class FoxCub : UnitBace
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
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.防御,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 3f,
            coolTime = 3,
            max = 5,
        };
        skills.skill.skill = MySkill;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        CommonSkillsManager.Instance.AttackNormal(my, target, skill, skillPara);
    }

    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        CommonSkillsManager.Instance.SkillRandomBlink(my, target, skill, skillPara);
    }
}
