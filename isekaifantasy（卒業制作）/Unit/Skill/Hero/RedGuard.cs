using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、RookieNightのスキルを制御する。
/// </summary>
public class RedGuard : UnitBace
{
    public override void InitSkill(SkillUnit skills)
    {
        base.InitSkill(skills);
        skills.passive = new Skill
        {
            skillAni = SkillAni.passive,
            skillName = SkillName.死亡游戏,
            spell = SpellType.no,
            coolTime = 10f,
        };
        skills.attack = new Skill
        {
            skillAni = SkillAni.attack,
            skillName = SkillName.扫射,
            spell = SpellType.loop,
            aiType = AiType.chase,
            isLookAt = true,
            canMove = true,
            spellDistance = 8f,
            max = 5,
            cost = 4,
            keepEffect = GameManager.Instance.GameConf.effectsKeep.RedGuardAttackKeep
        };
        skills.skill = new Skill
        {
            skillAni = SkillAni.skill,
            skillName = SkillName.翻滚,
            spell = SpellType.click,
            aiType = AiType.chase,
            isAssDir = true,
            spellDistance = 2f,
            cost = 0,
            coolTime = 5f,
        };
        skills.ultimate = new Skill
        {
            skillAni = SkillAni.ultimate,
            skillName = SkillName.大号铅弹,
            spell = SpellType.click,
            aiType = AiType.chase,
            spellDistance = 9f,
            cost = 0,
            coolTime = 10f,
        };
        skills.attack.skill = MyAttack;
        skills.skill.skill = MySkill;
        skills.ultimate.skill = MyUltimate;
        skills.passive.inIt = InitPassive;
        skills.passive.desIt = DesPassive;
    }


    public void MyAttack(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (target != null)
            my.LookAtPos(target.trs.position);
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.RedGuardBullet);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Solo, skillPara: skillPara,
                        attack: my.attack, speed: 20,
                        destroyDis: skill.spellDistance + 1,
                        h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
    }
    public void MySkill(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        my.SetMp(15);
        my.CancelRotate();
        if (!skillPara.isAssDir && target != null)
            my.trs.LookAt(target.trs);

        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        my.skillMov.Init(my.trs.forward, speed: 10f, stopDis: 4f, canThrough: true, s_Prefab: GameManager.Instance.GameConf.s_Effects.s_ArrowGoblinArcher);
    }

    public void MyUltimate(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.RedGuardUltimate);
        var pos = my.trs.position;
        pos.y += 1;
        ts.position = pos;
        ts.rotation = my.trs.rotation;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: 150 + my.magic * 0.75f, skillPara: skillPara,
            damageType: DamageType.magic,
            h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue,
            reSetDamage: GetDamage);

        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
    }

    private ReSetDamage GetDamage(UnitBace attacker, UnitBace target)
    {
        var data = new ReSetDamage();
        data.isDamage = true;
        data.damageType = DamageType.magic;

        var damage = 150 + attacker.magic * 0.75f;

        if (target != null && target.Owner != null)
            damage *= 3;

        data.damage = damage;
        return data;
    }


    public void InitPassive(UnitBace my, Skill skill)
    {
        BuffBase buff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.RedGuardPassive);
        buff.Init(BuffType.canNot, my, my, 0f, loopTime: 5f, skill: skill);
    }
    public void DesPassive(UnitBace my, Skill skill)
    {
        my.BuffDic
            [GameManager.Instance.GameConf.buff.RedGuardPassive.name]
            .DestroyByPool();
    }
}

