public class BuffSlow : BuffBase
{
    private float speed = -0.3f;
    private float speedAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if(value!=0) speed = value;
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
