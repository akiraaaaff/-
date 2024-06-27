using UnityEngine;

public class SpawnItemCtrl : MonoBehaviour
{
    private Transform trs;
    private int times;
    [SerializeField]
    private ItemType itemType;

    private void Awake()
    {
        trs = transform;
        if (trs.parent != null && trs.parent.parent != null)
        {
            RoomBace room = trs.parent.parent.GetComponent<RoomBace>();
            room.onAbleItems += Init;
            room.onDisableItems += Stop;
        }
        else Init();
    }
    public void Init()
    {
        times = 0;
        float time = Random.Range(1, 10);
        InvokeRepeating("Spawn", time, 10f);
    }
    private void Spawn()
    {
        times += 1;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.WaterSpawn);
        Transform ts = null;
        GameObject go = null;
        switch (itemType)
        {
            case ItemType.Hp:
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effectsItem.HpSpawn);
                go = GameManager.Instance.GameConf.effectsItem.HPoint;
                break;
            case ItemType.Mp:
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effectsItem.MpSpawn);
                go = GameManager.Instance.GameConf.effectsItem.MPoint;
                break;
        }
        ts.position = trs.position;
        ts.rotation = trs.rotation;
        EffectsBase eff = ts.GetComponent<EffectsBase>();
        eff.Init(o_Prefab: go);
        if (times >= 5) Stop();
    }
    public void Stop()
    {
        times = 0;
        CancelInvoke("Spawn");
    }
}
