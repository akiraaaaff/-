using UnityEngine;

public class BuffDarkMagePassive : BuffBase
{
    private ParticleSystem particle;
    private ParticleSystem.ShapeModule shape;
    private float thisTimeCount;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        trs.localEulerAngles = Vector3.zero;

        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
            shape = particle.shape;
        }
        shape.arc = 0;
        thisTimeCount = 0;
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        thisTimeCount += loopTime;
        var shape = particle.shape;
        shape.arc = 360 * thisTimeCount / (skill.coolTime - 0.3f);
        if (thisTimeCount >= skill.coolTime)
        {
            thisTimeCount = 0;
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.ExpandAura);
            ts.position = my.trs.position;
            EffectsBase eff = ts.GetComponent<EffectsBase>();
            eff.Init(my, buffAdd: GameManager.Instance.GameConf.buff.DarkAuraSlow,
                buffTime: 2f, buffType: BuffType.slow,
                range: DamageRange.cylinder, rangeHeight: 0.5f,
                color: new Color32(64, 0, 137, 255));
        }
    }
}
