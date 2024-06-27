using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomBace : MonoBehaviour
{
    public List<DoorBace> doorList = new List<DoorBace>();
    [SerializeField]
    private Transform spawnGroup;
    [HideInInspector]
    public List<Transform> spawnList = new List<Transform>();
    [HideInInspector]
    public Transform trs;
    [HideInInspector]
    public event Action onAbleItems;
    public event Action onDisableItems;
    public int index;
    
    public virtual void Init(int index)
    {
        trs = transform;
        this.index = index;
        foreach (Transform spawn in spawnGroup) spawnList.Add(spawn);
        foreach (DoorBace door in doorList) door.SetRoom(this);
    }

    public void OpenDoor()
    {
        Invoke("PlayVoice", 0.1f);
        foreach (DoorBace door in doorList) door.OpenDoor();
    }

    public void CloseDoor()
    {
        Invoke("PlayVoice", 0.1f);
        foreach (DoorBace door in doorList) door.CloseDoor();
    }
    private void PlayVoice()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.DoorOC);
    }

    public void AbleItems()
    {
        onAbleItems?.Invoke();
    }
    public void DisableItems()
    {
        onDisableItems?.Invoke();
    }
}
