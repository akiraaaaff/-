using System.Collections.Generic;
using UnityEngine;

public class RoomStickGroup : MonoBehaviour
{
    [HideInInspector]
    public Transform trs;
    [HideInInspector]
    public GameObject obj;
    public List<RoomStick> stickList;

    private void Awake()
    {
        trs = transform;
        obj = gameObject;
        AdModeManager.Instance.questBattle += OnBattle;
        AdModeManager.Instance.questNext += OnNext;
    }
    public void OnBattle()
    {
        trs.position=AdModeManager.Instance.nowRoom.trs.position;
        obj.SetActive(true);
    }

    public void OnNext()
    {
        obj.SetActive(false);
        foreach (var stick in stickList)
            stick.Off();
    }
    private void OnDestroy()
    {
        AdModeManager.Instance.questBattle -= OnBattle;
        AdModeManager.Instance.questNext -= OnNext;
    }
}
