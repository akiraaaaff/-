using UnityEngine;

public class SkeletonIceWarrior : UnitBace
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
            spellDistance = 3f,
            max = 5,
            coolTime = 1.2f,
        };
        skills.attack.skill = MyAttack;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.DarkFlame);
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;
        ts.localEulerAngles = new Vector3(0, ts.localEulerAngles.y, ts.localEulerAngles.z);
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line, skillPara: skillPara, attack: my.attack, speed: 5f, destroyDis: skill.spellDistance, damageType: DamageType.magic, i_Prefab: GameManager.Instance.GameConf.i_Effects.i_DarkFlame, o_Prefab: GameManager.Instance.GameConf.o_Effects.o_DarkFlame_small, h_Prefab: GameManager.Instance.GameConf.i_Effects.i_DarkFlame);
        my.skillMov.Init(my.trs.forward, stopDis: skill.spellDistance, speed: 5f);
    }
}
