using UnityEngine;

public class BuffRuneDeathControl : BuffBase
{
    private UnitBace unDead;
    private UnitBace unDeadTar;

    protected override void BuffAdd()
    {
        base.BuffAdd();
        if (buffLv == 1) 
        my.killEffects += BuffEffect;
        if (unDead != null)
        {
            if (buffLv == 2)
            {
                unDead.downHp = 0;
                unDead.attackSuck += 15;
            }
            if (buffLv == 3)
            {
                unDead.attack += unDead.attackIn * buffLv;
                unDead.AttackSpeed += 0.5f;
                unDead.hpMax += unDead.hpIn;
                unDead.Hp += unDead.hpIn;
                unDead.mpMax += 300;
                unDead.Mp += 300;
                unDead.Move += 200;
                unDead.SetMaterialColorDefult(GameManager.Instance.ColorIsDieLv3);
            }
        }
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.killEffects -= BuffEffect;
        if(unDeadTar!=null)
            unDeadTar.dieEffects -= BuffRun;
        unDeadTar = null;
        if (unDead != null)
            unDead.deathEffects -= OngangerDes;
        unDead = null;
    }

    private void BuffEffect(UnitBace target)
    {
        if (target == null)
            return;
        my.killEffects -= BuffEffect;
        unDeadTar = target;
        unDeadTar.dieEffects += BuffRun;

        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.SpawnMagic);
        Vector3 position = target.trs.position;
        position.y = 0;
        ts.position = position;
    }

    public override void BuffRun(UnitBace target)
    {
        Vector3 position = unDeadTar.trs.position;
        position.y = 0;

        var prafe = PlayerManager.Instance.dicAllUnitName[unDeadTar.unitName];
        Transform ts = PoolManager.Instance.GetObj(prafe);
        unDead = ts.GetComponent<UnitBace>();
        unDead.Init();
        unDead.nav.enabled = false;
        unDead.trs.position = position;
        unDead.SetGroup(GameManager.Instance.friendsList);
        unDead.Owner = my;
        unDead.ai.Init();
        DontDestroyOnLoad(unDead);
        unDead.dieEffects += OngangerDes;
        GameManager.Instance.friendsCanAtkList.Add(unDead.col);
        if (unDeadTar.isBoss)
        {
            unDead.hpMax += 1000;
            unDead.Hp += 1000;
        }

        unDeadTar.dieEffects -= BuffRun;
        unDeadTar = null;

        if (buffLv == 1)
        unDead.downHp = 30;
        if (buffLv == 2)
            unDead.attackSuck += 15;
        if (buffLv == 3)
        {
            unDead.attack += unDead.attackIn * buffLv;
            unDead.AttackSpeed += 0.5f;
            unDead.hpMax += unDead.hpIn;
            unDead.Hp += unDead.hpIn;
            unDead.mpMax += 300;
            unDead.Mp += 300;
            unDead.Move += 200;
            unDead.SetMaterialColorDefult(GameManager.Instance.ColorIsDieLv3);
        }
        else
        {
            unDead.SetMaterialColorDefult(GameManager.Instance.ColorIsDie);
        }
    }

    private void OngangerDes(UnitBace target)
    {
        unDead.dieEffects -= OngangerDes;
        unDead = null;
        my.killEffects += BuffEffect;
    }
}
