public class BuffDarkAuraSlow : BuffBase
{
    private float speed = -0.8f;
    private float speedAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        speed = SetMoveSpeedByMin(speed);
        speedAll += speed;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.MoveSpeed -= speedAll;
        speedAll = 0;
    }
}
