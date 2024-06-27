public class BuffRuneIceAreaSlow : BuffBase
{
    private float attackSpeed =-0.5f;
    private float speed = -0.3f;
    private float attackSpeedAll;
    private float speedAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        attackSpeed = SetAttackSpeedByMin(attackSpeed);
        speed = SetMoveSpeedByMin(speed);
        attackSpeedAll += attackSpeed;
        speedAll += speed;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.AttackSpeed -= attackSpeedAll;
        my.MoveSpeed -= speedAll;
        attackSpeedAll = 0;
        speedAll = 0;
    }
}
