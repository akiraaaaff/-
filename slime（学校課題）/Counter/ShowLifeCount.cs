using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ずっと、命カウントの値を表示する
/// </summary>
public class ShowLifeCount : MonoBehaviour 
{
    /// <summary>
    /// ずっと行う。テキストの内容を命カウンターの値に表示する
    /// </summary>
    void Update() 
    {
		GetComponent<Text>().text = GameCounter.LifeValue.ToString();
    }
}
