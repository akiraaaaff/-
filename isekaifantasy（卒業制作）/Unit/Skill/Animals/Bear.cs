using UnityEngine;
using System.Collections;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、Eggyのスキルを制御する。
/// </summary>
public class Bear : UnitBace
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
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.防御,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 3f,
            coolTime = 5f,
        };
        skills.attack.skill = MyAttack;
        skills.ultimate.skill = MyUltimate;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        CommonSkillsManager.Instance.AttackNormal(my, target, skill, skillPara);
    }
    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        CommonSkillsManager.Instance.SkillAreaShake(my, target, skill, skillPara);
    }
}
