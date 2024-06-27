using UnityEngine;
using UnityEngine.EventSystems;// タッチチェックに必要

/// <summary>
/// CanvasTuchのbackgroundに追加、ハンドルイベントクリックに継承
/// </summary>
public class TutorialTuchJump : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 画面がクリックされたらプレイヤーをジャンプさせ、CanvasTuchを削除
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("slime").GetComponent<TuchJump>().vy = 10;//上にジャンプす移動量を入れる
        GameObject.Find("slime").GetComponent<TuchJump>().vx = 1;//右にジャンプす移動量を入れる
        Destroy(transform.parent.gameObject);//CanvasTuchを削除
    }
}
