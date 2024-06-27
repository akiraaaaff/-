using System;
using System.Collections;
using UnityEngine;

public class ReLifeDialog : MonoBehaviour
{
    public GameObject Obj;



    public void Start()
    {
    }

    // ReLif-----------------
    private void ReLife()
    {
        Obj.SetActive(false);
        var my = PlayerManager.Instance.Hero;
        my.PlayAnime("die", true, false);
        my.PlayAnime("idle");
        my.Hp = 1;
        my.TakeDamage(my.hpMax, my, DamageType.heal);
        my.SetMp(my.mpMax);

        PlayerManager.Instance.InitPlayerBar(my);

        Joystick.Instance.gameObject.SetActive(true);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.WaterHeal);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.RevivalOK);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.localEulerAngles;
        ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.HpRestore);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.localEulerAngles;
        ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.MpRestore);
        ts.position = my.trs.position;
        ts.localEulerAngles = my.trs.localEulerAngles;
    }


    public void OnClickUserChoseToWatchAd()
    {
        //if (UIManager.Instance.reLifeAd.IsLoaded())
        //UIManager.Instance.reLifeAd.Show();
        ReLife();
    }

    public void OnClose()
    {
        Obj.SetActive(false);
        AdModeManager.Instance.GameOver();
    }
}
