using System.Collections.Generic;
using UnityEngine;

public class RuneGachaResultDialog : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private Transform runesRow = null;
    private List<RuneBaceRow> runeRowList = new List<RuneBaceRow>();
    private List<RuneName> viewRuneList = new List<RuneName>();



    private void Init()
    {
        RuneManager.Instance.InitDicRunesCanAdd();


        if (runeRowList.Count == 0)
        {
            foreach (Transform rune in runesRow)
            {
                runeRowList.Add(rune.GetComponent<RuneBaceRow>());
            }
        }
        for (int i = 0; i < runeRowList.Count; i++)
        {
            var rune = RuneManager.Instance.GetRuneByRandomInBattle();
            viewRuneList.Add(rune.name);
            runeRowList[i].Init(rune);
        }
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
        Init();
    }
}
