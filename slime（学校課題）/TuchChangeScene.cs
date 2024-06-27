using UnityEngine;
using UnityEngine.SceneManagement;// シーン切り替えに必要
using UnityEngine.EventSystems;// タッチチェックに必要

/// <summary>
/// Start,Clear,OverシーンのCanvasのAreaに追加、ハンドルイベントクリックに継承
/// </summary>
public class TuchChangeScene : MonoBehaviour, IPointerClickHandler
{
    public string sceneName;// チェンジするシーンの名：Inspectorで指定

    /// <summary>
    /// クリックしたらシーンを切り替える
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneName); //名でシーンをチェンジする
    }
}
