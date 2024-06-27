public class BuffBaku : BuffBase
{
    private float attackSpeed =-0.5f;
    private float speed = -0.5f;
    private float attackSpeedAll;
    private float speedAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1)
        {
            my.beAttackEffects += BuffRun;
            my.beSkillEffects += BuffRunSkill;
        }
        attackSpeed = SetAttackSpeedByMin(attackSpeed);
        speed = SetMoveSpeedByMin(speed);
        attackSpeedAll += attackSpeed;
        speedAll += speed;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.beAttackEffects -= BuffRun;
        my.beSkillEffects -= BuffRunSkill;
        my.AttackSpeed -= attackSpeedAll;
        my.MoveSpeed -= speedAll;
        attackSpeedAll = 0;
        speedAll = 0;
    }

    public override void BuffRun(UnitBace target)
    {
        if (my.BuffDic["FirebustSparkles"].owner != target) return;
        my.TakeDamage(3 + target.magic * 0.2f, target, DamageType.no);
    }
    public void BuffRunSkill(UnitBace target)
    {
        if (my.BuffDic["FirebustSparkles"].owner != target) return;
        my.TakeDamage(6 + target.magic * 0.5f, target, DamageType.magic);
        my.BuffDic["FirebustSparkles"].DestroyByPool();
    }
}
