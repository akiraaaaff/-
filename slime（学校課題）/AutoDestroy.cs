using UnityEngine;
/// <summary>
/// Canvasの突出しに追加、表示された時から一定の時間が経ったら自分を削除する
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    public float lifeTime; //表示時間：Inspectorで指定
    /// <summary>
    /// 最初に行う。表示時間が経ったら自分を削除する
    /// </summary>
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
