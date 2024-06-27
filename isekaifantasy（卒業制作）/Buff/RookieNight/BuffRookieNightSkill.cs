using UnityEngine;

public class BuffRookieNightSkill : BuffBase
{
    private ParticleSystem particle;
    private ParticleSystem.MainModule main;
    private float thisTimeCount;
    private readonly float damageReduction = 0.8f;
    private float damageReductionAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1)
        {
            my.beAttackEffects += BuffRun;
            my.beSkillEffects += BuffRun;
        }
        my.damageReduction += damageReduction;
        damageReductionAll += damageReduction;

        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
            main = particle.main;
        }
        SetMaxParticles(5);
        thisTimeCount = 0;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.beAttackEffects -= BuffRun;
        my.beSkillEffects -= BuffRun;
        my.damageReduction -= damageReductionAll;
        damageReductionAll = 0;

        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.ExpandAura);
        ts.position = my.trs.position;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: 40 + my.magic * 0.7f, skillPara : skillPara, damageType: DamageType.magic, h_Prefab: GameManager.Instance.GameConf.h_Effects.h_Hit_blue
            , buffAdd: GameManager.Instance.GameConf.buff.ArmorBreak, buffTime: 4f, buffType: BuffType.minus,buffValue:-15,
            range: DamageRange.cylinder, rangeHeight: 0.5f,
            color: new Color32(114,247,255,255));

        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.DamageHit);
        CameraBace.Instance.Shake(0.25f, 0.1f);
    }

    public override void BuffRun(UnitBace target)
    {
        BuffBase tempBuff = my.CheckBuffToAdd(GameManager.Instance.GameConf.buff.Slow);
        tempBuff.Init(BuffType.slow, my, target, 1f, value: -0.5f);
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        thisTimeCount += loopTime;
        if (thisTimeCount >= skill.keepTime / 5)
        {
            thisTimeCount = 0;
            SetMaxParticles(main.maxParticles - 1);
        }
    }

    private void SetMaxParticles(int max)
    {
        main.maxParticles = max;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}