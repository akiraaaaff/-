using System.Collections.Generic;
using UnityEngine;

public class BuffSkeletonPoisonMagePassive : BuffBase
{
    protected override void BuffLoop()
    {
        base.BuffLoop();
        List<UnitBace> tempList = null;
        var tar = my;
        var hp_min = tar.Hp;
        float tarCheckDis = 6;

        for (var j = 0; j < 2; j++)
        {
            if (j == 0)
                tempList = GameManager.Instance.friendsBaceList;
            else
            {
                tarCheckDis = 2;
                tempList = GameManager.Instance.enemyBaceList;
            }


            if (tempList != null && tempList.Count > 0)
            {
                for (var i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] == my)
                        continue;

                    float distance = Vector3.Distance(my.trs.position, tempList[i].trs.position);
                    if (distance > tarCheckDis)
                        continue;

                    if (tempList[i].Hp < hp_min)
                    {
                        hp_min = tempList[i].Hp;
                        tar = tempList[i];
                    }
                    else
                        // buffLvCheck
                        if (tempList[i].Hp == hp_min)
                    {
                        var isAdd = false;
                        var buff = tempList[i].CheckBuffToGet(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison);
                        if (buff != null)
                        {
                            var buffLvNow = buff.buffLv;
                            var buffLvTar = 0;
                            buff = tar.CheckBuffToGet(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison);
                            if (buff != null)
                                buffLvTar = buff.buffLv;
                            if (buffLvNow <= buffLvTar)
                                isAdd = true;
                        }
                        else isAdd = true;
                        if (isAdd)
                        {
                            hp_min = tempList[i].Hp;
                            tar = tempList[i];
                        }
                    }
                }
            }
        }

        var tempBuffType = BuffType.minus;
        if (owner.friendsGroup == tar.friendsGroup)
            tempBuffType = BuffType.plus;

        var haveBuff = tar.CheckBuff(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison.name);
        BuffBase tempBuff = tar.CheckBuffToAdd(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison);

        var isEnemyBuff = false;
        if (haveBuff)
            isEnemyBuff = my.friendsGroup != tempBuff.owner.friendsGroup;

        tempBuff.Init(tempBuffType, tar, my, 10f, buffLvMax: 5, loopTime: 2f);
        if (haveBuff && isEnemyBuff)
            tempBuff.buffLv = 1;
    }
}