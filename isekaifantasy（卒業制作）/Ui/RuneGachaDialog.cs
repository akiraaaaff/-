using System;
using System.Collections;
using UnityEngine;

public class RuneGachaDialog : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private RuneGachaResultDialog resultDialog;



    // Gacha-----------------
    private void Gacha()
    {
        OnClose();
        resultDialog.OnOpen();
    }


    public void OnClickUserChoseToWatchAd()
    {
        //if (UIManager.Instance.gachaAd.IsLoaded())
        //UIManager.Instance.gachaAd.Show();
        Gacha();
    }

    public void OnClose()
    {
        Time.timeScale = 1;
        obj.SetActive(false);
    }

    public void OnOpen()
    {
        Time.timeScale = 0;
        obj.SetActive(true);
    }
}
