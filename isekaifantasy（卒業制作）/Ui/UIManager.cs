using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// uiの全てのボタンの機能の制御
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIGameOver gameOverModal;
    public RuneModal runeModal;
    public RuneChangeModal runeChangeModal;
    public OptionModal optionModal;
    public RuneViewInBattle runeViewInBattle;
    public UITips tips;
    public UISpawnShow SpawnShowRow;
    public AiAutoButton aiAutoButton;
    public GameObject buttonMaster;
    public GameObject buttonAuto;
    public Joystick joystick;
    public GraphicRaycaster raycaster;

    public void Init()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
            joystick.Init();
            //InitGachaAd();
            //InitReLifeAd();
        }
    }
    public void GameOver(bool isClear = false)
    {
        gameOverModal.Init(isClear);
    }
}
