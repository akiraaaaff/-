using UnityEngine;

public class BuffRuneAttackEnchant : BuffBase
{
    private int useTimes;
    private ParticleSystem particle;
    private ParticleSystem.MainModule main;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useSkillEffects += OnSkill;
        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
            main = particle.main;
        }
        SetMaxParticles(0);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        useTimes = 0;
        my.useAttackEffects -= OnAttack;
        my.attackOnceEffects -= BuffUse;
        my.useSkillEffects -= OnSkill;
        SetMaxParticles(0);
    }

    private void BuffUse(UnitBace target, Transform trs)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.AttackEnchant);
        ts.position = trs.position;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(my, attack: 60 + my.magic * value, damageType: DamageType.magic);
    }
    private void OnSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (useTimes <= 0)
        {
            my.useAttackEffects += OnAttack;
            my.attackOnceEffects += BuffUse;
            SetMaxParticles(10);
        }
        useTimes = buffLv;
    }
    private void OnAttack(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (useTimes > 0)
        {
            useTimes--;
            if (useTimes == 0)
            {
                my.attackOnceEffects -= BuffUse;
                my.useAttackEffects -= OnAttack;
                SetMaxParticles(0);
            }
        }
    }
    private void SetMaxParticles(int max)
    {
        main.maxParticles = max;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
