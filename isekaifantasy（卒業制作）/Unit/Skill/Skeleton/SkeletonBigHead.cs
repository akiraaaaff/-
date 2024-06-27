using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、Monstersのスキルを制御する。
/// </summary>
public class SkeletonBigHead : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.ダークフレーム,
            spell = SpellType.click,
            aiType = AiType.keepHome,
            spellDistance = 6f,
            max = 5,
        };
        skills.attack.skill = MyAttack;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        CommonSkillsManager.Instance.BulletNormal(my, target, skill, skillPara);
    }
}

