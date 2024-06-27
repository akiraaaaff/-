using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private GameObject textWorldOver;
    [SerializeField]
    private GameObject textWorldClear;
    [SerializeField]
    private Transform runesRow = null;
    private List<RuneBaceRow> runeRowList = new List<RuneBaceRow>();
    private List<RuneName> viewRuneList = new List<RuneName>();
    //AiAuto------------------------------------------------------------------------------
    public float aiChoseRune;
    //------------------------------------------------------------------------------

    public void Init(bool isClear = false)
    {
        if (runeRowList.Count == 0)
        {
            foreach (Transform rune in runesRow)
            {
                runeRowList.Add(rune.GetComponent<RuneBaceRow>());
            }
        }
        for (int i = 0; i < runeRowList.Count; i++)
        {
            if (i < UIManager.Instance.runeModal.runeInBattlePosManger.runeGotList.Count)
            {
                if (viewRuneList.Count > 0 && viewRuneList.Contains(UIManager.Instance.runeModal.runeInBattlePosManger.runeGotList[i].name))
                {
                    runeRowList[i].Exit();
                }
                else
                {
                    viewRuneList.Add(UIManager.Instance.runeModal.runeInBattlePosManger.runeGotList[i].name);
                    runeRowList[i].Init(UIManager.Instance.runeModal.runeInBattlePosManger.runeGotList[i]);
                }
            }
            else
            {
                runeRowList[i].Exit();
            }
        }
        obj.SetActive(true);
        if (isClear)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.WinMusic);
            textWorldClear.SetActive(true);
            textWorldOver.SetActive(false);
        }
        else
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.LoseMusic);
            textWorldClear.SetActive(false);
            textWorldOver.SetActive(true);
        }
        //AiAuto------------------------------------------------------------------------------
        aiChoseRune = 3f;
        //------------------------------------------------------------------------------
    }
    //AiAuto------------------------------------------------------------------------------
    private void Update()
    {
        if (aiChoseRune > 0f && obj.activeSelf)
        {
            aiChoseRune -= Time.fixedUnscaledDeltaTime;
            if (aiChoseRune <= 0f)
            {
                aiChoseRune = 3f;
                AIAutoChoseRune();
            }
        }
    }
    private void AIAutoChoseRune()
    {
        if (PlayerManager.Instance.allAuto &&
        !UIManager.Instance.runeChangeModal.obj.activeSelf)
        {
            if (viewRuneList.Count == 0)
                ReStart();
            else
            {
                int num = Random.Range(0, viewRuneList.Count);
                runeRowList[num].OnClickInRuneSaveCanvas();
            }
            viewRuneList.Clear();
        }
    }
    //------------------------------------------------------------------------------

    public void ReStart()
    {
        if (AdModeManager.Instance != null)
        {
            AdModeManager.Instance.GameOver();
        }
        else
            LoadScene();
    }

    public void LoadScene()
    {
        aiChoseRune = 0f;

        PlayerManager.Instance.Hero = null;
        PlayerManager.Instance.Target = null;
        GameManager.Instance.ClearAllList();
        UIManager.Instance.runeModal.ClearRune();
        UIManager.Instance.runeModal.runeInBattlePosManger.runeClear();
        UIManager.Instance.SpawnShowRow.Hide();
        UIManager.Instance.runeModal.Hide();
        UIManager.Instance.optionModal.Hide();
        UIManager.Instance.buttonMaster.SetActive(false);
        UIManager.Instance.buttonAuto.SetActive(false);
        Joystick.Instance.gameObject.SetActive(false);
        obj.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
