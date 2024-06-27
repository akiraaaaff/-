using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、GobliArcherのスキルを制御する。
/// </summary>
public class GoblinArcher : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.passive = new Skill
        {
            skillAni = SkillAni.passive,
            skillName = SkillName.ダブル射撃,
            canMoveDontBreak = true,
            spell = SpellType.no,
            coolTime = 10,
        };
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.射て,
            spell = SpellType.click,
            canMoveDontBreak = true,
            aiType = AiType.chase,
            spellDistance = 10f,
            max = 5,
        };
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.射撃ノックバック,
            spell = SpellType.click,
            aiType = AiType.runAway,
            isAssDir = true,
            spellDistance = 10f,
            cost = 15,
            coolTime = 5f,
        };
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.マジック矢,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 11f,
            cost = 50,
            coolTime = 7f,
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
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.ArrowGoblinArcher);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Solo, skillPara: skillPara,
                        attack: my.attack, speed: 20,
                        destroyDis: skill.spellDistance+3,
                        i_Prefab: GameManager.Instance.GameConf.i_Effects.i_ArrowGoblinArcher,
                        o_Prefab: GameManager.Instance.GameConf.o_Effects.o_ArrowGoblinArcher,
                        o_PrefabStay: true,
                        h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
    }
    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        my.CancelRotate();
        if (skillPara.isAssDir)
        {
            if (!skillPara.isDouble)
                my.trs.localEulerAngles += new Vector3(0, 180, 0);
        }
        else if (target != null)
        {
            my.trs.LookAt(target.trs);
        }
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.ArrowGoblinArcherBig);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line, skillPara: skillPara, attack: 20 + my.attack, speed: 20, destroyDis: skill.spellDistance, i_Prefab: GameManager.Instance.GameConf.i_Effects.i_ArrowGoblinArcherBig, o_Prefab: GameManager.Instance.GameConf.o_Effects.o_ArrowGoblinArcherBig, o_PrefabStay: true, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
        my.skillMov.Init(-bulletBase.trs.forward, speed: 10f, stopDis: 4f, canThrough: true, s_Prefab: GameManager.Instance.GameConf.s_Effects.s_ArrowGoblinArcher);
    }
    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.ArrowGoblinArcherMagic);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line, skillPara: skillPara, attack: (70 + my.magic * 0.85f) * (1 + skillPara.chargePara / skill.chargePara*0.6f), damageType: DamageType.magic, speed: 30, destroyDis: skill.spellDistance, i_Prefab: GameManager.Instance.GameConf.i_Effects.i_ArrowGoblinArcherBigMagic, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_ArrowGoblinArcherBigMagic);
        skill.nowChargePara = 0;
    }
    public void InitUltimate(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.GoblinArcherUltimate);
        buff.Init(BuffType.canNot, my, my, 0f, loopTime: 1f, parent: my.bulletPosRight, skill: skill);
    }
    public void DesUltimate(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.GoblinArcherUltimate.name]
            .BuffSkillRemove(skill);
    }

    public void MyPassive(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        my.CheckBuffStateToRun(GameManager.Instance.GameConf.buff.GoblinArcherPassive, state: BuffState.wait);
    }
    public void InitPassive(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.GoblinArcherPassive);
        buff.Init(BuffType.canNot, my, my, 0, buffState: BuffState.wait, loopTime: 1f, skill: skill);
    }
    public void DesPassive(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.GoblinArcherPassive.name]
            .DestroyByPool();
    }
}

