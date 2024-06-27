using System.Collections;
using UnityEngine;

/// <summary>
/// カメラに追加し、固定視点でプレイヤーと一定の距離を保て上下で移動する
/// </summary>
public class CameraBace : MonoBehaviour
{
    public static CameraBace Instance;

    protected float speed = 2f;
    protected float zMin;
    protected float zMax;
    protected Vector3 targetPos;
    protected Transform trs;

    protected void Awake()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
        }
        trs = transform;
        zMin = -17;
        zMax = -1;
    }

    private void LateUpdate()
    {
        if (PlayerManager.Instance.Hero == null) return;
        var z = PlayerManager.Instance.Hero.trs.position.z - 15f;
        if (z > zMax) z = zMax;
        else if (z < zMin) z = zMin;

        targetPos = new Vector3(0, 20, z);
        trs.position = Vector3.Lerp(trs.position, targetPos, speed * Time.deltaTime);
    }

    public virtual void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    protected virtual IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = trs.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            trs.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.fixedDeltaTime;

            yield return null;
        }

        trs.localPosition = pos;
    }
}
