using UnityEngine;

public class RoomStick : MonoBehaviour
{
    private Transform trs;
    [SerializeField]
    private float height = 1f;
    [SerializeField]
    private float speed = 1f;

    private float stopTime;
    private bool isOn;
    private bool isOpen;
    private Vector3 OnPos;
    private Vector3 OffPos;

    private void Start()
    {
        trs = transform;
        OffPos = trs.localPosition;
        OnPos = new Vector3(OffPos.x, OffPos.y + height, OffPos.z);
    }
    public void Off()
    {
        stopTime = 1;
        isOn = false;
        isOpen = false;
        trs.localPosition = OffPos;
    }

    private void FixedUpdate()
    {
        if (stopTime > 0)
            stopTime -= Time.deltaTime;

        if (isOpen)
        {
            float distance = Vector3.Distance(trs.localPosition, OnPos);
            if (distance > 0.8f) trs.localPosition = Vector3.Lerp(trs.localPosition, OnPos, Time.deltaTime * 5 * speed);
            else
            {
                trs.localPosition = OnPos;
                isOpen = false;
                isOn = true;
            }
        }
    }

    private void OnTriggerEnter(Collider tar)
    {
        if (isOn)
            return;
        if (isOpen)
            return;
        if (stopTime > 0)
            return;
        if (GameManager.Instance.enemyList.Contains(tar) ||
            GameManager.Instance.friendsList.Contains(tar))
            TakeDamage(tar);
    }

    private void TakeDamage(Collider tar)
    {
        isOpen = true;
        UnitBace target = tar.GetComponent<UnitBace>();
        target.TakeDamage(30, type: DamageType.attack);
    }
}
