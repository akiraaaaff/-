using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、RookieNightのスキルを制御する。
/// </summary>
public class RookieNight : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.passive = new Skill
        {
            skillAni = SkillAni.passive,
            skillName = SkillName.不滅意志,
            overrClip = aniOverr["passive"],
            spell = SpellType.no,
            coolTime = 180f,
            keepTime = 8f,
        };
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.斬撃,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 3.5f,
            max = 5,
        };
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.防御,
            spell = SpellType.loop,
            aiType = AiType.chase,
            spellDistance = 3f,
            canMove = true,
            cost = 20,
            coolTime = 8f,
            keepTime = 10f,
        };
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.連斬,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 3f,
            cost = 50,
            coolTime = 5f,
            chargePara = 3f,
        };
        skills.attack.skill = MyAttack;
        skills.skill.skill = MySkill;
        skills.ultimate.skill = MyUltimate;
        skills.ultimate.inIt = InitUltimate;
        skills.ultimate.desIt = DesUltimate;
        skills.passive.skill = MyPassive;
        skills.passive.inIt = InitPassive;
        skills.passive.desIt = DesPassive;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.Slash_blue);
        ts.position = skillPara.bulletPos.position;
        ts.localEulerAngles = new Vector3(my.trs.eulerAngles.x, my.trs.eulerAngles.y, skillPara.bulletPos.eulerAngles.z - skillPara.angle);
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: my.attack, skillPara: skillPara, damageType: DamageType.attack, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
    }

    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (!my.CheckBuff(GameManager.Instance.GameConf.buff.RookieNightSkill.name))
            AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.RookieNightSkill);
        buff.Init(BuffType.canNot, my, my, 1f, skill: skill, skillPara: skillPara);
    }

    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        if (skillPara.chargePara > 0)
        {
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.Bullet_Slash_red);
            ts.position = skillPara.bulletPos.position;
            ts.localEulerAngles = new Vector3(my.trs.eulerAngles.x, my.trs.eulerAngles.y, skillPara.bulletPos.eulerAngles.z - skillPara.angle);
            float destroyDis = skillPara.chargePara * 2.5f;
            BulletBase bulletBase = ts.GetComponent<BulletBase>();
            bulletBase.Init(my, target, BulletType.Line, speed: 13f, destroyDis: destroyDis, attack: 120 + my.magic * 1.5f, skillPara: skillPara, damageType: DamageType.magic, i_Prefab: GameManager.Instance.GameConf.i_Effects.i_Slash_red, o_Prefab: GameManager.Instance.GameConf.o_Effects.o_Slash_red, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Slash_red);
            BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.RookieNightUltimate);
            skill.nowChargePara = 0;
        }
        else
        {
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.Slash_red);
            ts.position = skillPara.bulletPos.position;
            ts.localEulerAngles = new Vector3(my.trs.eulerAngles.x, my.trs.eulerAngles.y, skillPara.bulletPos.eulerAngles.z - skillPara.angle);
            EffectsBase eff = ts.GetComponent<EffectsBase>();
            eff.Init(my, attack: 120 + my.magic * 1.5f, skillPara: skillPara, damageType: DamageType.magic, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Slash_red);
        }
    }
    public void InitUltimate(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.RookieNightUltimate);
        buff.Init(BuffType.canNot, my, my, 0f, loopTime: 1f, parent: my.bulletPosRight, skill: skill);
    }
    public void DesUltimate(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.RookieNightUltimate.name]
            .BuffSkillRemove(skill);
    }

    public void MyPassive(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        my.CheckBuffStateToRun(GameManager.Instance.GameConf.buff.RookieNightPassive, state: BuffState.wait);
    }
    public void InitPassive(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.RookieNightPassive);
        buff.Init(BuffType.canNot, my, my, 0f, loopTime: 0, buffState: BuffState.wait, skill: skill);
    }
    public void DesPassive(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.RookieNightPassive.name]
            .DestroyByPool();
    }
}

