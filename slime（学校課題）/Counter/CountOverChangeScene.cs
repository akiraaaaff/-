using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;	// シーン切り替えに必要

/// <summary>
/// LifeCountに追加、カウントが最終値なら、シーンを切り換える
/// </summary>
public class CountOverChangeScene : MonoBehaviour 
{
    public int TimeCount = 0; // タイムカウンターの最終値：Inspectorで指定
    public int ClrarCount = 1; // クリアカウンターの最終値：Inspectorで指定
    public int FinishCount = 0; // カウンターの最終値：Inspectorで指定
    public string ClearSceneName; // 終了クリアシーン名：Inspectorで指定
    public string FinishSceneName; // 終了シーン名：Inspectorで指定

    bool once;//一回しか実行しないbool型変数onceを宣言

    /// <summary>
    /// ずっと行う（一定時間ごとに）。 ターゲットカウンター値チェック
    /// </summary>
    void FixedUpdate() 
    {
        //もし、チュートリアルに最初の鮭寿司を食べたら
        if (GameManager.playLeve == 0 && GameCounter.LifeValue == 2 && GameManager.start == false && GameManager.over == false && once == false)
        {
            once = true;//一回しか実行しない
            StartCoroutine(Getyume());//チュートリアルのゲームターゲット表示
        }

        //プレイ状態の時にタッチの動作をチェック
        if (GameManager.start == true && GameManager.over == false)
        {
            if (GameCounter.TargetValue == ClrarCount)// ターゲットカウンターが最終値になったら
            {
                GameCounter.TimeValue = 3;//タイム制限を3に表示
                StartCoroutine(SwitchScene(ClearSceneName));//シーンの切り換えをスタート
                GameManager.playLeve += 1;//難易度+1
            }
            else//じゃなかった場合
            {
                // 命カウンターかタイムカウンターが最終値になったら
                if (GameCounter.LifeValue <= FinishCount || GameCounter.TimeValue <= TimeCount)
                {
                    GameCounter.TimeValue = 0;// タイムカウンターの値ゼロにする
                    StartCoroutine(SwitchScene(FinishSceneName));//シーンの切り換えをスタート
                    //プレイヤーの死亡アニメをプレイする
                    GameObject.Find("slime").GetComponent<Animator>().SetTrigger("die");
                }
            }
        }
	}

    /// <summary>
    /// 名前でシーンを切り換えると他の動作
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumerator SwitchScene(string name)
    {
        //プレイヤーの衝突を取消
        GameObject.Find("slime").GetComponent<Rigidbody2D>().isKinematic = true;
        GameManager.over = true;//プレイヤーの死亡、ゲームオーバーにする

        if (GameManager.playLeve == 0)// もし、初プレイだったら
        {
            GameManager.playLeve += 1;//難易度+1、一回プレイしたら、もうチュートリアル表示しない
        }

        yield return new WaitForSecondsRealtime(3f);//3秒待ち

        if (name == ClearSceneName)// もし、ゲームクリアだったら
        {
            //プレイヤーの死亡アニメをプレイする
            GameObject.Find("slime").GetComponent<Animator>().SetTrigger("die");
            yield return new WaitForSecondsRealtime(3f);//3秒待ち
        }

        SceneManager.LoadScene(name);// シーンを切り換える
    }

    /// <summary>
    /// チュートリアルとUIのゲームターゲット表示
    /// </summary>
    /// <returns></returns>
    IEnumerator Getyume()
    {
        yield return new WaitForSeconds(1.5f);//1.5秒待ち
        //チュートリアルのセリフyumeを表示
        GameObject.Find("Canvas").transform.Find("yume").gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);//3秒待ち
        //UIのターゲットIcon表示
        GameObject.Find("Canvas").transform.Find("target").Find("Image").gameObject.SetActive(true);
        //CanvasTuch表示
        GameObject.Find("EventSystem").transform.Find("CanvasTuch").gameObject.SetActive(true);
    }
}
