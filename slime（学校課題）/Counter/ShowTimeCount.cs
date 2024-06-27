using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ずっと、タイムカウントの値を表示する
/// </summary>
public class ShowTimeCount : MonoBehaviour
{
    /// <summary>
    /// ずっと行う、テキストの内容を変えて表示する
    /// </summary>
    void Update()
    {
        if(GameCounter.TimeValue <= 0)// もし、タイムカウンターの値が0になったら
        {
            GetComponent<Text>().text = ("Die");// テキストをDieと表示する
        }
        else//じゃなかった場合
        {
            GetComponent<Text>().text = ((int)GameCounter.TimeValue).ToString();// タイムカウンターの値を表示する
        }


        if (GameCounter.TimeValue <= 5)// もし、タイムカウンターの値が5以下になったら
        {
            GetComponent<Animator>().SetBool("die", true);//アニメーションdieをプレイする。
        }
        else//じゃなかった場合
        {
            GetComponent<Animator>().SetBool("die", false);//アニメーションdieをプレイしない。
        }
    }
}
