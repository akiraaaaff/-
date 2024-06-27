using System.Collections;
using UnityEngine;

public class BuffGoblinArcherPassive : BuffBase
{
    private ParticleSystem particle;
    private ParticleSystem.MainModule main;
    private float thisTimeCount;
    [SerializeField]
    private GameObject startPar;

    public override void BuffRun(UnitBace target)
    {
        if (buffState == BuffState.wait)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.ButtonClick);
            buffState = BuffState.start;
            SetMaxParticles(20);
            skill.nowCoolTime = skill.coolTime;
            my.useSkillEffects += OnSkill;
            my.useAttackEffects += OnSkill;
        }
        else
        {
            my.useSkillEffects -= OnSkill;
            my.useAttackEffects -= OnSkill;
            buffState = BuffState.wait;
            SetMaxParticles(0);
        }
    }

    protected override void BuffAdd()
    {
        base.BuffAdd();

        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
            main = particle.main;
        }
        SetMaxParticles(0);
        thisTimeCount = 0;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useSkillEffects -= OnSkill;
        my.useAttackEffects -= OnSkill;
    }

    private void SetMaxParticles(int max)
    {
        main.maxParticles = max;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        startPar.SetActive(max != 0);
    }
    protected override void BuffLoop()
    {
        base.BuffLoop();
        thisTimeCount += loopTime;
        if (thisTimeCount >= skill.nowCoolTime && buffState == BuffState.wait && !my.isSkilling && (my.ai.my == null || (my.ai.my != null && my == PlayerManager.Instance.Hero && PlayerManager.Instance.isPlayerMove > 0)))
        {
            thisTimeCount = 0;
            my.Spelld(my.skills.passive, aiCanBreak: true);
        }
    }
    private void OnSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (!my.doubleSkillOnly)
            my.doubleSkillOnly = true;
        else
            return;
        if (skill.skillAni == SkillAni.attack)
            my.SkillColdDownOne(this.skill, 8);
        BuffRun(null);
        var tempPara = new SkillPara();
        tempPara.CopyData(skillPara);
        tempPara.isDouble = true;
        StartCoroutine(DoubleSkill(target, skill, tempPara));
    }

    private IEnumerator DoubleSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        yield return new WaitForSeconds(0f);
        my.doubleSkillOnly = false;
        yield return new WaitForSeconds(GameManager.Instance.DoubleSkillWaitTime);
        skill.skill?.Invoke(my, target, skill, skillPara);
    }
}