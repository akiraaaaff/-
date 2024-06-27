using UnityEngine;

public class BuffRuneRevival : BuffBase
{
    [SerializeField]
    private GameObject shiled;
    [SerializeField]
    private GameObject revival;
    private int useTimes;
    private bool isRevival;


    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1)
            my.lifeDeathEffects += BuffWaiToStart;
        if (!isRevival)
        {
            shiled.SetActive(true);
            revival.SetActive(false);
        }
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.lifeDeathEffects -= BuffWaiToStart;
        my.lifeDieEffects -= BuffWaiToStart2;
        shiled.SetActive(false);
        revival.SetActive(false);
        useTimes = 0;
        isRevival = false;
    }

    public void BuffWaiToStart(UnitBace target)
    {
        if (!my.lifeDeathOnly)
            my.lifeDeathOnly = true;
        else
            return;
        if (isRevival)
            return;
        my.Hp = 1;
        useTimes++;
        GameManager.Instance.canNotAtkList.Add(my.col);
        my.PlayAnime("die", true, true);
        Invoke(nameof(Revival), 3f);
        my.lifeDeathEffects -= BuffWaiToStart;
        my.lifeDieEffects += BuffWaiToStart2;
        shiled.SetActive(false);
        revival.SetActive(true);
        isRevival = true;
    }
    public void BuffWaiToStart2(UnitBace target)
    {
    }
    public void Revival()
    {
        my.lifeDeathOnly = false;
        my.PlayAnime("die", true, false);
        my.PlayAnime("idle");
        my.TakeDamage(50+my.hpMax * value, my, DamageType.heal);

        my.SetMp((int)(150 + my.mpMax * value));
        my.lifeDieEffects -= BuffWaiToStart2;
        GameManager.Instance.canNotAtkList.Remove(my.col);
        Joystick.Instance.gameObject.SetActive(true);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.WaterHeal);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.RevivalOK);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.localEulerAngles;
        ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.HpRestore);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.localEulerAngles;
        ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.MpRestore);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.localEulerAngles;
        revival.SetActive(false);
        isRevival = false;
        if (useTimes >= buffLv)
            DestroyByPool();
        else
        {
            my.lifeDeathEffects += BuffWaiToStart;
            shiled.SetActive(true);
        }
    }
}
