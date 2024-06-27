public class BuffSkel : BuffBase
{
    private int attack;
    private readonly float speed = 0.2f;
    private int attackAll;
    private float speedAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        attack = (int)(my.attack * 0.2f);
        my.attack += attack;
        my.MoveSpeed += speed;
        attackAll += attack;
        speedAll += speed;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.attack -= attackAll;
        my.MoveSpeed -= speedAll;
        attackAll = 0;
        speedAll = 0;
    }
}
