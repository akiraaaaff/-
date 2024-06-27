using System.Collections.Generic;
using UnityEngine;

public class SkeletonFireKing : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.passive = new Skill
        {
            skillAni = SkillAni.passive,
            skillName = SkillName.燃焼の魂,
            spell = SpellType.no,
        };
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.炎斬,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 3.5f,
            max = 5,
        };
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.炎爆,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 10f,
            cost = 50,
            coolTime = 12f,
        };
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.炎骨大葬,
            spell = SpellType.click,
            aiType = AiType.runAway,
            cost = 150,
            coolTime = 60f,
            effList = new List<Transform>(),
        };
        skills.attack.skill = MyAttack;
        skills.skill.skill = MySkill;
        skills.ultimate.skill = MyUltimate;
        skills.passive.inIt = InitPassive;
        skills.passive.desIt = DesPassive;
    }

    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.Slash_red);
        ts.position = skillPara.bulletPos.position;
        ts.localEulerAngles = new Vector3(my.trs.eulerAngles.x, my.trs.eulerAngles.y, skillPara.bulletPos.eulerAngles.z - skillPara.angle);
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: my.attack, skillPara: skillPara, damageType: DamageType.attack, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Slash_red);
        if (skill.nowNextIndex == 0)
        {
            ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.FireCleaveRed);
            ts.position = my.trs.position;
            ts.localEulerAngles = my.trs.eulerAngles;
            eff = ts.GetComponent<EffectsBase>();
            eff.Init(my, attack: 30 + my.magic * 0.6f, skillPara: skillPara, damageType: DamageType.attack, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Slash_red);
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
        }
    }
    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.DamageHit);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.FireBlast);
        if (target == null)
        {
            ts.position = my.trs.position + (my.trs.rotation * new Vector3(0f, 0f, 1f));
            ts.rotation = my.trs.rotation;
        }
        else
        {
            ts.position = target.trs.position;
            ts.rotation = target.trs.rotation;
        }
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: (60 + my.magic * 0.7f), skillPara: skillPara,
            damageType: DamageType.magic,
            clip: GameManager.Instance.GameConf.sounds.MagicBurst,
                color: new Color32(255, 47, 0, 255));
        if (AdModeManager.Instance != null)
        {
            var waitTime = 0f;
            foreach (Transform childTs in AdModeManager.Instance.nowRoom.spawnList)
            {
                waitTime += 0.1f;
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.FireBlast);
                ts.position = childTs.position;
                ts.rotation = childTs.rotation;
                eff = ts.GetComponent<EffectsBase>();
                eff.Init(my, attack: 60 + my.magic * 0.7f, skillPara: skillPara,
                    damageType: DamageType.magic, waitTime: waitTime,
                    color: new Color32(255, 47, 0, 255));
            }
        }
    }
    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.ButtonClick);
        var times = 1;
        if (skillPara.power > 0.3)
            times++;
        if (skillPara.power > 0.7)
            times++;
        for (var i = 0; i < times; i++)
        {
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.SpawnMagicShort);
            Vector3 position = my.trs.position + Random.insideUnitSphere * 3;
            position.y = 0;
            ts.position = position;

            ts = PoolManager.Instance.GetObj(PlayerManager.Instance.dicAllUnitName[UnitName.SkeletonAngry]);
            UnitBace friend = ts.GetComponent<UnitBace>();
            friend.Init();
            friend.hpMax = (int)(friend.hpMax * 0.5f);
            friend.Hp = friend.hpMax;
            friend.restoreHp = 0;
            friend.downHp = 10;
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
    }

    public void InitPassive(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.SkeletonFireKingPassive);
        buff.Init(BuffType.canNot, my, my, 0f, skill: skill);
    }
    public void DesPassive(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.SkeletonFireKingPassive.name]
            .DestroyByPool();
    }
}
