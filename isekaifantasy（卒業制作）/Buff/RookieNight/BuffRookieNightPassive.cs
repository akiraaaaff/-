using UnityEngine;

public class BuffRookieNightPassive : BuffBase
{
    private ParticleSystem particle;
    private ParticleSystem.MainModule main;

    public override void BuffRun(UnitBace target)
    {
        buffState = BuffState.start;
        my.cantRun = false;
        GameManager.Instance.canNotAtkList.Remove(my.col);
        Joystick.Instance.gameObject.SetActive(true);
        Invoke("BuffCold", skill.keepTime);
        SetMaxParticles(20);
        if (buffLv == 1) my.lifeDeathEffects += BuffStart;
        Invoke("BuffColdBefore", skill.keepTime * 0.68f);
    }

    private void BuffColdBefore()
    {
        SetMaxParticles(10);
    }

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1) my.lifeDeathEffects += BuffWaiToStart;

        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
            main = particle.main;
        }
        SetMaxParticles(5);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.lifeDeathEffects -= BuffWaiToStart;
        my.lifeDieEffects -= BuffWaiToStart2;
        my.lifeDeathEffects -= BuffStart;
    }
    public void BuffWaiToStart(UnitBace target)
    {
        if (!my.lifeDeathOnly)
            my.lifeDeathOnly = true;
        else
            return;
        my.Hp = 1;

        my.cantRun = true;
        GameManager.Instance.canNotAtkList.Add(my.col);
        my.PlayAnime("die", true, true);
        if (buffLv == 1) my.lifeDeathEffects -= BuffWaiToStart;
        if (buffLv == 1) my.lifeDieEffects += BuffWaiToStart2;
    }

    public void BuffWaiToStart2(UnitBace target)
    {
        my.PlayAnime("die", true, false);
        my.Spelld(my.skills.passive);
        my.lifeDieEffects -= BuffWaiToStart2;
    }

    public void BuffStart(UnitBace target)
    {
        my.Hp = 1;
    }

    private void BuffCold()
    {
        my.lifeDeathOnly = false;
        buffState = BuffState.cold;
        SetMaxParticles(1);
        Invoke("BuffWait", skill.coolTime);
        Invoke("BuffWaitBefore", skill.coolTime * 0.9f);
        if (buffLv == 1) my.lifeDeathEffects -= BuffStart;
    }

    private void BuffWaitBefore()
    {
        SetMaxParticles(3);
    }

    private void BuffWait()
    {
        buffState = BuffState.wait;
        SetMaxParticles(5);
        if (buffLv == 1) my.lifeDeathEffects += BuffWaiToStart;
    }

    private void SetMaxParticles(int max)
    {
        main.maxParticles = max;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}