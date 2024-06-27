using System.Collections.Generic;
using UnityEngine;

public class BuffGhostAcidClawPassive : BuffBase
{
    private static readonly float buffTime = 5;

    [SerializeField]
    private GameObject point1;
    [SerializeField]
    private GameObject point2;
    [SerializeField]
    private GameObject point3;

    private float runTime;
    private float upTime;
    private bool attacked;
    private bool isRun;

    private void BuffAddStart()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.ButtonClick);
        if (isRun)
        {
            if (buffLv < buffLvMax)
                buffLv++;
            point3.SetActive(!isRun);
            point3.SetActive(isRun);
            return;
        }
        isRun = true;
        SetParticles();
        my.useAttackEffects += OnAttack;
    }

    private void BuffAddWait()
    {
        if (buffLv > 1)
        {
            buffLv--;
            return;
        }
        my.useAttackEffects -= OnAttack;
        isRun = false;
        attacked = false;
        SetParticles();
    }

    protected override void BuffAdd()
    {
        base.BuffAdd();
        isRun = false;
        attacked = false;
        runTime = 0;
        upTime = 0;
        SetParticles();
    }
    public override void BuffRun(UnitBace target)
    {
        base.BuffRun(target);
        upTime = buffTime;
        BuffAddStart();
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useAttackEffects -= OnAttack;
        isRun = false;
        attacked = false;
        runTime = 0;
        upTime = 0;
        SetParticles();
    }

    private void SetParticles()
    {
        point1.SetActive(isRun);
        point2.SetActive(isRun);
        point3.SetActive(isRun);
    }
    protected override void BuffLoop()
    {
        if (isRun && !attacked)
            return;
        base.BuffLoop();

        runTime += loopTime;
        if (runTime >= skill.coolTime && (!isRun || upTime > 0))
        {
            runTime = 0;
            BuffAddStart();
        }
        else
        if (runTime >= buffTime && isRun && upTime <= 0)
        {
            runTime = 0;
            BuffAddWait();
        }

        if (upTime > 0)
        {
            upTime -= loopTime;
            if (upTime <= 0)
            {
                upTime = 0;
                BuffAddWait();
            }
        }
    }
    private void OnAttack(UnitBace target, Skill skill, SkillPara skillPara)
    {
        attacked = true;

        var angele = my.trs.localEulerAngles;
        var angeleY = angele.y;
        var count = buffLv * 2;
        var groupDamageList = new List<UnitBace>();
        for (var i = 0; i < count; i++)
        {
            if (i >= (count / 2))
            {
                if (i == (count / 2))
                    angeleY = angele.y;
                angeleY += 15;
            }
            else
                angeleY -= 15;

            Transform ts = PoolManager.Instance.GetObj
                   (GameManager.Instance.GameConf.bullets.Bullet_Slash_Green);
            ts.localEulerAngles = new Vector3(angele.x, angeleY, angele.z);
            ts.position = my.trs.position + (Quaternion.Euler(ts.forward) * ts.rotation * new Vector3(0f, 2.5f, 1.5f));

            BulletBase bulletBase = ts.GetComponent<BulletBase>();
            bulletBase.Init(my, target, BulletType.Line, attack: my.attack, speed: 15f
                , skillPara: skillPara, destroyDis: skill.spellDistance - 3f
                , groupDamageList: groupDamageList, damageAttenuation: 0.3f);
        }
    }
}