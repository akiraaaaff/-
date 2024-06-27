using UnityEngine;

public class BuffRedGuardPassive : BuffBase
{
    private UnitBace lastTargrt;


    protected override void BuffRemove()
    {
        base.BuffRemove();
        lastTargrt = null;
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        SetTarget();
    }

    private void AddTarget(UnitBace tar)
    {
        BuffBase tempBuff = tar.CheckBuffToAdd(GameManager.Instance.GameConf.
            buff.RedGuardPassiveTar);

        tempBuff.Init(BuffType.minus, tar, my, skill.coolTime,
            loopTime: 1f);
        lastTargrt = tar;

        var buff = tempBuff.GetComponent<BuffRedGuardPassiveTar>();
        buff.SetEndAction(ReSetTarget);
    }

    private void ReSetTarget()
    {
        if (my == null)
            return;
        if (my.Target == null || my.Target == lastTargrt || my.Target.Hp <= 0)
        {
            FindTarget();
            return;
        }

        AddTarget(my.Target);
    }

    private void SetTarget()
    {
        if (lastTargrt != null || my == null)
            return;
        if (my.Target == null || my.Target == lastTargrt || my.Target.Hp <= 0)
        {
            FindTarget();
            return;
        }

        AddTarget(my.Target);
    }

    private void FindTarget()
    {
        var tempList = GameManager.Instance.enemyBaceList;
        if (my.friendsGroup == GameManager.Instance.enemyList)
            tempList = GameManager.Instance.friendsBaceList;

        UnitBace tar = null;
        var hp_min = 9999999;
        float tarCheckDis = 15;

        for (var i = 0; i < tempList.Count; i++)
        {
            if (tempList[i] == lastTargrt)
                continue;

            float distance = Vector3.Distance(my.trs.position, tempList[i].trs.position);
            if (distance > tarCheckDis)
                continue;

            if (tempList[i].Hp < hp_min && tempList[i].Hp > 1)
            {
                hp_min = tempList[i].Hp;
                tar = tempList[i];
            }
        }

        lastTargrt = null;

        if (tar == null)
            return;
        AddTarget(tar);
    }
}