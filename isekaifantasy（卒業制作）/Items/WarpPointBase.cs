using System.Collections;
using UnityEngine;

public class WarpPointBase : MonoBehaviour
{
    private ItemGroup itemGroup;
    private Transform trs;
    private int id;
    private int count;

    private void Awake()
    {
        itemGroup = transform.parent.GetComponent<ItemGroup>();
        trs = transform;
        id = trs.GetSiblingIndex();
        count = trs.parent.childCount;
    }
    private void OnTriggerEnter(Collider tar)
    {
        if (!itemGroup.executedList.Contains(tar) &&
            (GameManager.Instance.enemyList.Contains(tar) ||
            GameManager.Instance.friendsList.Contains(tar)))
        {
            //AiAuto------------------------------------------------------------------------------
            if (tar == PlayerManager.Instance.Hero.col && PlayerManager.Instance.battleAuto&&PlayerManager.Instance.isPlayerMove<=0 && AdModeManager.Instance.QuestStatus != QuestStatus.battle)
                return;
            //------------------------------------------------------------------------------
            Warp(tar);
        }
    }

    private void Warp(Collider tar)
    {
        int next= id+1;
        if (id>= count-1)
            next = 0;
        itemGroup.executedList.Add(tar);
        tar.transform.position = trs.parent.GetChild(next).position;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.ButtonClick);
        if(tar==PlayerManager.Instance.Hero.col)
            StartCoroutine(ReWarp(tar,0.1f));
        else StartCoroutine(ReWarp(tar));
    }
    private IEnumerator ReWarp(Collider tar,float time=10f)
    {
        yield return new WaitForSeconds(time);
        itemGroup.executedList.Remove(tar);
    }
}
