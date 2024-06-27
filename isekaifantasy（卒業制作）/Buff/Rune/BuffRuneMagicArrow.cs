using UnityEngine;

public class BuffRuneMagicArrow : BuffBase
{
    [SerializeField]
    private Transform Point1;
    [SerializeField]
    private Transform Point2;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1)
        {
            Point2.gameObject.SetActive(false);
            my.useAttackEffects += BuffAttack;
        }
        if (buffLv == 2)
        {
            Point2.gameObject.SetActive(true);
        }
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useAttackEffects -= BuffAttack;
    }

    private void BuffAttack(UnitBace target, Skill skill, SkillPara skillPara)
    {
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.ArrowMagicArrowLight);
        if (target == null)
            Point1.eulerAngles = my.trs.eulerAngles;
        else
            Point1.LookAt(target.trs);
        Point1.eulerAngles = new Vector3(0, Point1.eulerAngles.y, Point1.eulerAngles.z);
        ts.position = Point1.position;
        ts.rotation = Point1.rotation;
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Solo, skillPara: skillPara, attack: 5 + my.attack * value + my.magic * value * 0.5f, damageType: DamageType.magicArrow, speed: 20, destroyDis: 11);
        if (buffLv <= 1)
            return;

        ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.bullets.ArrowMagicArrowLight);
        if (target == null)
            Point2.eulerAngles = my.trs.eulerAngles;
        else
            Point2.LookAt(target.trs);
        Point2.eulerAngles = new Vector3(0, Point2.eulerAngles.y, Point2.eulerAngles.z);
        ts.position = Point2.position;
        ts.rotation = Point2.rotation;
        bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(my, target, BulletType.Solo, skillPara: skillPara, attack: 5 + my.attack * value + my.magic * value * 0.5f, damageType: DamageType.magicArrow, speed: 20, destroyDis: 11);
    }
}
