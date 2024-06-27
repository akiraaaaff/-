using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、SkeletonMageGreenのスキルを制御する。
/// </summary>
public class SkeletonPoisonMage : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.passive = new Skill
        {
            skillAni = SkillAni.passive,
            skillName = SkillName.毒ガス,
            spell = SpellType.no,
            coolTime = 8,
        };
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.毒玉,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 9f,
            max = 5,
        };
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.スケルトン召喚,
            spell = SpellType.click,
            aiType = AiType.runAway,
            spellDistance = 15f,
            cost = 50,
            coolTime = 2f,
            effList = new List<Transform>(),
        };
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.毒爆発,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 9f,
            cost = 60,
            coolTime = 30f,
        };
        skills.attack.skill = MyAttack;
        skills.skill.skill = MySkill;
        skills.ultimate.skill = MyUltimate;
        skills.ultimate.inIt = InitUltimate;
        skills.ultimate.desIt = DesUltimate;
        skills.passive.inIt = InitPassive;
        skills.passive.desIt = DesPassive;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj
            (GameManager.Instance.GameConf.bullets.PoisonFlame);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line, attack: my.attack
            , skillPara: skillPara, destroyDis: skill.spellDistance
            , i_Prefab: GameManager.Instance.GameConf.i_Effects.i_PoisonFlame
            , o_Prefab: GameManager.Instance.GameConf.o_Effects.o_PoisonFlame_small
            , h_Prefab: GameManager.Instance.GameConf.h_Effects.h_PoisonFlame);
    }
    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.ButtonClick);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.SpawnMagicShort);
        Vector3 position = my.trs.position + Random.insideUnitSphere * 2;
        position.y = 0;
        ts.position = position;

        ts = PoolManager.Instance.GetObj(PlayerManager.Instance.dicAllUnitName[UnitName.SkeletonAngry]);
        UnitBace friend = ts.GetComponent<UnitBace>();
        friend.Init();
        friend.attack = (int)(friend.attack * 0.5f);
        friend.restoreHp = 0;
        friend.nav.enabled = false;
        friend.trs.position = position;
        friend.SetGroup(my.friendsGroup);
        friend.Owner = my;
        friend.ai.Init();
        DontDestroyOnLoad(friend);

        friend.swpanSkill = skill;
        if (skill.effList.Count >= 4)
            skill.effList[0].GetComponent<UnitBace>().ToDeathUseEff();
        skill.effList.Add(ts);
    }
    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.PoisonBlast);
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: 50 + my.magic * 1f, skillPara: skillPara, damageType: DamageType.magic, range: DamageRange.cylinder, rangeHeight: 2f
            , h_Prefab: GameManager.Instance.GameConf.h_Effects.h_PoisonFlame
            , reSetDamage: GetDamage);
    }


    private ReSetDamage GetDamage(UnitBace attacker, UnitBace target)
    {
        var data = new ReSetDamage();
        data.isDamage = true;
        data.damageType = DamageType.magic;

        var buffLv = 0;
        var buff = target.CheckBuffToGet(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison);
        if (buff != null && attacker.friendsGroup == buff.owner.friendsGroup)
            buffLv = buff.buffLv;
        data.damage= 50 + 30 * buffLv + attacker.magic * 1f;

        return data;
    }

    public void InitUltimate(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.SkeletonPoisonMageUltimate);
        buff.Init(BuffType.canNot, my, my, 0f, loopTime: 0, parent: my.bulletPosRight, skill: skill);
    }
    public void DesUltimate(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.SkeletonPoisonMageUltimate.name]
            .BuffSkillRemove(skill);
    }

    public void InitPassive(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassive);
        buff.Init(BuffType.canNot, my, my, 0, buffState: BuffState.wait, loopTime: skill.coolTime, skill: skill);
    }
    public void DesPassive(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassive.name]
            .DestroyByPool();
    }
}
