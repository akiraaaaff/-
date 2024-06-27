using UnityEngine;

public class BuffRuneManaAura : BuffBase
{
    private EffectsBase effect;
    private ParticleSystem particle;
    private ParticleSystem.EmissionModule emmiss;
    private ParticleSystem.Burst burst;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        trs.localEulerAngles = Vector3.zero;

        if (particle == null)
        {
            effect= GetComponent<EffectsBase>();
            particle = GetComponent<ParticleSystem>();
            emmiss = particle.emission;
            burst = emmiss.GetBurst(0);
        }
        burst.count = buffLv;
        emmiss.SetBurst(0, burst);
        effect.Init(my, attack: value + my.mpMax * 0.03f,destroyTime:0, damageType: DamageType.no, range: DamageRange.cylinder, rangeHeight: 0.5f, loopTime: 0.5f);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        effect.ReSet();
    }
    protected override void BuffLoop()
    {
        base.BuffLoop();
        effect.SetAttackDamage(value + my.mpMax * 0.03f);
    }
}
