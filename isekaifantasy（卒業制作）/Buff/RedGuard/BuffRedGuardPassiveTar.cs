using System;
using UnityEngine;

public class BuffRedGuardPassiveTar : BuffBase
{
    int totalDamage;
    Action endAction;
    RedGuardPassiveText effText;
    RedGuardPassiveText EffText
    {
        get
        {
            if (effText == null)
            {
                Transform ts = PoolManager.Instance.GetObj
                    (GameManager.Instance.GameConf.effects.RedGuardPassiveText);
                ts.position = trs.position;
                effText = ts.GetComponent<RedGuardPassiveText>();
            }
            return effText;
        }
    }


    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.beDamageEffects += GetDamage;

        EffText.SetParent(trs, (int)destroyTime);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.beDamageEffects -= GetDamage;
        if (totalDamage > 0)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);


            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.
                effects.RedGuardPassiveExplosionWaterBlack);
            ts.position = my.trs.position;


            EffectsBase eff = ts.GetComponent<EffectsBase>();
            eff.Init(owner, attack: totalDamage, skillPara: skillPara,
                damageType: DamageType.no,
                target: my);
        }
        totalDamage = 0;
        EffText.DestroyByPool();
        endAction?.Invoke();
        endAction = null;
    }


    protected override void BuffLoop()
    {
        base.BuffLoop();
        EffText.SetText((int)(destroyTime - timeCount), totalDamage);
    }

    private void GetDamage(UnitBace target, int damage)
    {
        totalDamage += damage;
        EffText.SetText((int)(destroyTime - timeCount), totalDamage);
        if (totalDamage >= my.Hp)
            DestroyByPool();
    }

    public void SetEndAction(Action action)
    {
        endAction = action;
    }
}