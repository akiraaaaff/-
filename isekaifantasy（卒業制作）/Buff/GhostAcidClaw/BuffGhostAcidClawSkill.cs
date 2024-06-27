public class BuffGhostAcidClawSkill : BuffBase
{

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (!GameManager.Instance.canNotAtkList.Contains(my.col))
            GameManager.Instance.canNotAtkList.Add(my.col);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        GameManager.Instance.canNotAtkList.Remove(my.col);
    }
}