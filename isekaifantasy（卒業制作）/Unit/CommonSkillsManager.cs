using UnityEngine;

public class CommonSkillsManager
{
    private readonly Color blue = Color.blue;
    private readonly Color red = Color.red;

    private static CommonSkillsManager instance;

    public static CommonSkillsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CommonSkillsManager();
            }
            return instance;
        }

    }


    public void AttackNormal(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        //獲得Colliderの最近点
        if (target == null) return;
        Vector3 tarNearestRadiusPos = target.col.ClosestPoint(my.trs.position);
        float tarRadius = Vector3.Distance(tarNearestRadiusPos, target.trs.position);
        float distance = Vector3.Distance(my.trs.position, target.trs.position) - tarRadius;
        Vector3 targetDir = target.trs.position - my.trs.position; //ターゲット座標と自身の差
        float rotateY = Vector3.Angle(my.trs.forward, targetDir); // 自身とターゲット座標のアングル
        if (rotateY < 90 && distance <= my.attackDistance)
        {
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
            ts.position = target.beHitPos.position;
            ts.rotation = my.trs.rotation;
            target.TakeDamage(my.attack * skillPara.power, my, DamageType.attack);
        }
    }
    public void AttackThirsty(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        //獲得Colliderの最近点
        if (target == null) return;
        Vector3 tarNearestRadiusPos = target.col.ClosestPoint(my.trs.position);
        float tarRadius = Vector3.Distance(tarNearestRadiusPos, target.trs.position);
        float distance = Vector3.Distance(my.trs.position, target.trs.position) - tarRadius;
        Vector3 targetDir = target.trs.position - my.trs.position; //ターゲット座標と自身の差
        float rotateY = Vector3.Angle(my.trs.forward, targetDir); // 自身とターゲット座標のアングル
        if (rotateY < 90 && distance <= my.attackDistance)
        {
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
            ts.position = target.beHitPos.position;
            ts.rotation = my.trs.rotation;
            target.TakeDamage(my.attack * skillPara.power, my, DamageType.attack);
            my.TakeDamage(20 * skillPara.power, my, DamageType.heal);
        }
    }

    public void BulletNormal(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.MissileCircleRed);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Solo,
            skillPara: skillPara, attack: my.attack,
            destroyDis: skill.spellDistance,
            o_Prefab: GameManager.Instance.GameConf.o_Effects.o_MissileRedSmall,
            color: skillClor);
    }
    public void BulletArrow(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.ArrowCommon);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Solo,
            skillPara: skillPara, attack: my.attack,
            collisionDestroy: true, speed: 20,
            destroyDis: skill.spellDistance,
            h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue,
            color: skillClor);
    }

    public void Bullet3Small(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        var angle = my.trs.eulerAngles;

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        for (int i = 0; i < 3; i++)
        {
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.MissileCircleRed);
            ts.position = skillPara.bulletPos.position;
            ts.localEulerAngles = new Vector3(angle.x, angle.y + (i - 1) * 20, angle.z);
            BulletBase bulletBase = ts.GetComponent<BulletBase>();
            bulletBase.Init(my, target, BulletType.Solo,
                skillPara: skillPara, attack: 10 + my.attack * 0.4f,
                damageType: DamageType.magic, destroyDis: skill.spellDistance,
                o_Prefab: GameManager.Instance.GameConf.o_Effects.o_MissileRedSmall,
            color: skillClor);
        }
    }

    public void BulletLineMiddle(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.MissileConeRedTail);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line,
            skillPara: skillPara, attack: my.attack,
            speed: 5f, destroyDis: skill.spellDistance,
            o_Prefab: GameManager.Instance.GameConf.o_Effects.o_MissileRedMiddle,
            color: skillClor);
    }

    public void BulletAoeLarge(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.MissileStarRed);
        ts.position = skillPara.bulletPos.position;
        ts.rotation = my.trs.rotation;

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Aoe,
            skillPara: skillPara, attack: my.attack,
            speed: 5f, destroyDis: skill.spellDistance,
            o_Prefab: GameManager.Instance.GameConf.o_Effects.o_MissileRedLarge,
            color: skillClor);
    }

    public void SkillBlink(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        my.trs.position = my.trs.position +
            (Quaternion.Euler(my.trs.position - target.trs.position) *
            my.trs.rotation * new Vector3(0f, 0f, 2f));
        my.LookAtPos(target.trs.position);
    }

    public void SkillRandomBlink(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Vector3 position = my.trs.position + Random.insideUnitSphere * 4;
        position.y = 0;
        my.trs.position = position;
        my.LookAtPos(target.trs.position);
    }

    public void SkillCharge(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        my.skillMov.Init(my.trs.forward, stopDis: skill.spellDistance,
            speed: 5f, attack: my.attack * skillPara.power,
            damageType: DamageType.attack,
            h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue);
    }

    public void SkillChargeBullet(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.MissileConeRedTail);
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;
        ts.localEulerAngles = new Vector3(0, ts.localEulerAngles.y, ts.localEulerAngles.z);

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Line,
            skillPara: skillPara, attack: my.attack,
            speed: 5f, destroyDis: skill.spellDistance,
            damageType: DamageType.magic,
            i_Prefab: GameManager.Instance.GameConf.i_Effects.i_DarkFlame,
            o_Prefab: GameManager.Instance.GameConf.o_Effects.o_MissileRedMiddle,
            color: skillClor);

        my.skillMov.Init(my.trs.forward, stopDis: skill.spellDistance, speed: 5f);
    }

    public void SkillSpawnTreasure(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        Transform ts = null;
        GameObject go = null;
        var ranNum = Random.Range(0, 2);
        switch (ranNum)
        {
            case 0:
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effectsItem.HpSpawn);
                go = GameManager.Instance.GameConf.effectsItem.HPoint;
                break;
            case 1:
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effectsItem.MpSpawn);
                go = GameManager.Instance.GameConf.effectsItem.MPoint;
                break;
        }
        ts.position = my.trs.position;
        ts.rotation = my.trs.rotation;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(o_Prefab: go);
    }

    public void SkillAreaShake(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.ExpandAura);
        ts.position = my.trs.position;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: my.attack, skillPara: skillPara, damageType: DamageType.attack,
            h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue,
            range: DamageRange.cylinder, rangeHeight: 0.5f,
            color: skillClor);

        CameraBace.Instance.Shake(0.25f, 0.1f);
    }

    public void SkillFarCircleArea(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.FireBlast);
        ts.position = target.trs.position;
        ts.rotation = target.trs.rotation;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: my.attack, skillPara: skillPara,
            damageType: DamageType.magic,
            clip: GameManager.Instance.GameConf.sounds.MagicBurst,
            color: skillClor);
    }

    public void SkillShortCleave(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);

        var skillClor = red;
        if (my.friendsGroup == GameManager.Instance.friendsList)
            skillClor = blue;

        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.StarCleave);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.eulerAngles;
        var eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: my.attack, skillPara: skillPara,
            damageType: DamageType.magic,
            h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Slash_red,
            color: skillClor);
    }
}
