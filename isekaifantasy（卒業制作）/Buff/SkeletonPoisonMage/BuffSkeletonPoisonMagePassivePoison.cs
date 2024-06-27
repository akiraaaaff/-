using System.Collections.Generic;
using UnityEngine;

public class BuffSkeletonPoisonMagePassivePoison : BuffBase
{
    private float damage;
    private List<Collider> executedList;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useSkillEffects += SkillUse;
        executedList = new List<Collider>();
        InvokeRepeating(nameof(RefreshCollider), 2, 2);
        gameObject.layer = 2;
    }

    private void SkillUse(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (owner.friendsGroup == my.friendsGroup)
            return;
        Invoke(nameof(DestroyBuff), 0f);
    }
    private void DestroyBuff()
    {
        my.BuffDic[name].DestroyByPool();
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useSkillEffects -= SkillUse;
        CancelInvoke();
    }

    protected override void BuffLoop()
    {
        base.BuffLoop();
        damage = buffLv;
        if (owner.friendsGroup == my.friendsGroup) my.TakeDamage(damage, owner, DamageType.heal);
        else my.TakeDamage(damage, owner, DamageType.poison);
    }

    private void AddBuff(UnitBace target, BuffType tempBuffType)
    {
        var haveBuff = target.CheckBuff(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison.name);
        BuffBase tempBuff = target.CheckBuffToAdd(GameManager.Instance.GameConf.buff.SkeletonPoisonMagePassivePoison);

        var isEnemyBuff = false;
        if (haveBuff)
            isEnemyBuff = owner.friendsGroup != tempBuff.owner.friendsGroup;

        tempBuff.Init(tempBuffType, target, owner, 10f, buffLvMax: 5, loopTime: 2f);
        if (haveBuff && isEnemyBuff)
            tempBuff.buffLv = 1;
    }

    private void OnTriggerEnter(Collider tar)
    {
        if (executedList.Contains(tar)) return;
        if ((owner.enemyGroup.Contains(tar) ||
            owner.friendsGroup.Contains(tar)) &&
            my.col != tar)
        {
            executedList.Add(tar);
            var target = tar.GetComponent<UnitBace>();
            var tempBuffType = BuffType.minus;
            if (owner.friendsGroup.Contains(tar))
                tempBuffType = BuffType.plus;
            AddBuff(target, tempBuffType);
        }
    }

    private void RefreshCollider() => executedList.Clear();
}
