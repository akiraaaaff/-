public class BuffArmorBreak : BuffBase
{
    private int armor = -10;
    private int armorAll;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if(value!=0) armor = (int)value;
        my.armor += armor;
        armorAll += armor;
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.armor -= armorAll;
        armorAll = 0;
    }
}
