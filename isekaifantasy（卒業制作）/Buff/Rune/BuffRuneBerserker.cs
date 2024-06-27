using UnityEngine;

public class BuffRuneBerserker : BuffBase
{
    private const float StillTime = 5;
    private float thisTimeCount;
    private float allValue;
    private int nowLv;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.killEffects += BuffRun;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.killEffects -= BuffRun;
        my.AttackSpeed -= allValue;
        nowLv = 0;
        allValue = 0;
        thisTimeCount = 0;
    }

    public override void BuffRun(UnitBace target)
    {
        if (nowLv<buffLv)
        {
            nowLv++;
            my.AttackSpeed += value;
            allValue += value;
        }
        thisTimeCount = 0;
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        thisTimeCount += loopTime;
        if (thisTimeCount >= StillTime)
        {
            thisTimeCount = 0;
            my.AttackSpeed -= allValue;
            nowLv = 0;
            allValue = 0;
        }
    }
}
