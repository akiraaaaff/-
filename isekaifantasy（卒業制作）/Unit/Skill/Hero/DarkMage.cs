using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、DarkMageのスキルを制御する。
/// </summary>
public class DarkMage : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.passive = new Skill
        {
            skillAni = SkillAni.passive,
            skillName = SkillName.ダークオーラ,
            spell = SpellType.no,
            spellDistance = 3f,
            coolTime = 5,
        };
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.ダークフレーム,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 10f,
            max = 5,
        };
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.ダーク連射,
            spell = SpellType.click,
            aiType = AiType.random,
            spellDistance = 7f,
            cost = 50,
            coolTime = 6f,
        };
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.ダークマキシマム,
            spell = SpellType.keep,
            aiType = AiType.chase,
            spellDistance = 4f,
            cost = 100,
            coolTime = 20f,
            keepTime = 3f,
            keepEffect = GameManager.Instance.GameConf.effectsKeep.DarkMageUltimateKeep
        };
        skills.attack.skill = MyAttack;
        skills.skill.skill = MySkill;
        skills.ultimate.skill = MyUltimate;
        skills.passive.inIt = InitPassive;
        skills.passive.desIt = DesPassive;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.DarkFlame);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Aoe, attack: my.attack, skillPara: skillPara, destroyDis: skill.spellDistance - 3, i_Prefab: GameManager.Instance.GameConf.i_Effects.i_DarkFlame, o_Prefab: GameManager.Instance.GameConf.o_Effects.o_DarkFlame, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_DarkFlame);
    }

    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.DarkFlame);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = skillPara.bulletPos.rotation;
        if (skillPara.power == 1f)
        {
            ts.rotation = my.trs.rotation;
        }
        skillPara.damageEffects += Backmove;
        ts.localEulerAngles = new Vector3(0, ts.localEulerAngles.y, ts.localEulerAngles.z);
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line, attack: 120f + my.magic * 0.7f, skillPara: skillPara, destroyDis: skill.spellDistance, damageType: DamageType.magic, i_Prefab: GameManager.Instance.GameConf.i_Effects.i_DarkFlame, o_Prefab: GameManager.Instance.GameConf.o_Effects.o_DarkFlame_small, h_Prefab: GameManager.Instance.GameConf.i_Effects.i_DarkFlame);
    }
    private void Backmove(UnitBace target, Transform trs)
    {
        if (target == null)
            return;
        target.skillMov.Init(trs.forward, speed: 5f, stopDis: 3f, moveAnimator: "getHit", isBool: true);
    }

    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.DarkMageUltimateBirth);
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: 120 + my.magic * 1f, skillPara: skillPara, damageType: DamageType.magic, range: DamageRange.hemisphere, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_DarkFlame);

        float tempTime = 2 + skillPara.keepTime * 10;

        Transform ts2 = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.DarkMageUltimate);
        ts2.position = my.trs.position;
        ts2.rotation = my.trs.rotation;
        EffectsBase eff2 = ts2.GetComponent<EffectsBase>();
        eff2.Init(my, destroyTime: tempTime, attack: 15 + my.magic * 0.05f, skillPara: skillPara, damageType: DamageType.no, range: DamageRange.cone, rangeInnerRadius: 3.4f, rangeHeight: 2.5f, loopTime: 0.2f, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_DarkFlame);

        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicStay);
    }

    public void InitPassive(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.DarkMagePassive);
        buff.Init(BuffType.canNot, my, my, 0f, skill: skill);
    }
    public void DesPassive(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.DarkMagePassive.name]
            .DestroyByPool();
    }
}

