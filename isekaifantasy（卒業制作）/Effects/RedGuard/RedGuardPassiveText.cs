using UnityEngine;
using UnityEngine.UI;

public class RedGuardPassiveText : MonoBehaviour
{
    [HideInInspector]
    public Transform trs;
    private Transform parent;
    [SerializeField]
    private Text text;

    private void Awake()
    {
        trs = transform;
    }

    private void Update()
    {
        var pos = parent.position;
        trs.position = new Vector3(pos.x, pos.y + 2.3f, pos.z);
    }

    public void SetParent(Transform parent, int time)
    {
        this.parent = parent;
        SetText(time, 0);
    }

    public void SetText(int time, int damage)
    {
        text.text = string.Format("{0:00}: {1:000}", time, damage);
    }

    /// <summary>
    /// 不表示時に消す
    /// </summary>
    public void DestroyByPool()
    {
        PoolManager.Instance.PushObj(trs);
    }
}
