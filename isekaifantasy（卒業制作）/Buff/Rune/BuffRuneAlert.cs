using UnityEngine;

public class BuffRuneAlert : BuffBase
{
    private float allAttackSpeed;
    private int allMoveSpeed;

    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.hpChangedEffects += BuffChange;
        BuffChange();
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.hpChangedEffects -= BuffChange;
        my.AttackSpeed -= allAttackSpeed;
        allAttackSpeed = 0;
        my.Move -= allMoveSpeed;
        allMoveSpeed = 0;
    }

    public void BuffChange()
    {
        var hpPercent = (float)my.Hp / my.hpMax;
        var nowAttackSpeed = value - hpPercent;
        var nowMoveSpeed = (int)(nowAttackSpeed*100);
        my.AttackSpeed -= allAttackSpeed;
        my.AttackSpeed += nowAttackSpeed;
        allAttackSpeed = nowAttackSpeed;
        my.Move -= allMoveSpeed;
        my.Move += nowMoveSpeed;
        allMoveSpeed = nowMoveSpeed;
    }
}
