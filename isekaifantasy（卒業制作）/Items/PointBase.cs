using UnityEngine;
using UnityEngine.AI;

public class PointBase : MonoBehaviour
{
    private Transform trs;
    [SerializeField]
    private NavMeshAgent nav;
    [SerializeField]
    private ItemType itemType;
    [SerializeField]
    private int point=50;

    private void Awake()
    {
        trs = transform;
        if (nav != null)
            nav.enabled = false;
    }
    private void OnEnable()
    {
        Invoke("Able",0.1f);
    }
    private void Able()
    {
        if (nav != null)
        {
            nav.enabled = false;
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.WaterDown);
            float x = Random.Range(-1.25f, 1.25f);
            float z = Random.Range(-1.25f, 1.25f);
            trs.position = new Vector3
                (trs.position.x+x, 0, trs.position.z+z);
            nav.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider tar)
    {
        if (GameManager.Instance.enemyList.Contains(tar) ||
            GameManager.Instance.friendsList.Contains(tar))
        {
            TakeHeal(tar);
            DestroyByPool();
        }
    }

    private void TakeHeal(Collider tar)
    {
        UnitBace target = tar.GetComponent<UnitBace>();
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.WaterHeal);
        Transform ts = null;
        switch (itemType)
        {
            case ItemType.Hp:
                target.TakeDamage(point, null, DamageType.heal);
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.HpRestore);
                break;
            case ItemType.Mp:
                target.SetMp(point);
                ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.MpRestore);
                break;
        }
        ts.position = target.trs.position;
        ts.rotation = target.trs.rotation;
    }

    protected virtual void DestroyByPool()
    {
        if (nav != null)
            nav.enabled = false;
        PoolManager.Instance.PushObj(trs);
    }
}
