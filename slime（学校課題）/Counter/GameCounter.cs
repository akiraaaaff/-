using System.Collections;
using UnityEngine;

/// <summary>
/// LifeCountに追加、カウンター本体、ゲーム難易度の処理
/// </summary>
public class GameCounter : MonoBehaviour 
{
    public static float TimeValue; // タイムカウンターの値
    public static int LifeValue; // 命カウンターの値
    public static int TargetValue; // ターゲットカウンターの値

    public int startTimeCount = 20; // タイムカウンター初期値：Inspectorで指定
    public int startLifeCount = 1; // 命カウンター初期値：Inspectorで指定
    public int startTargetCount = 0; // ターゲットカウンター初期値：Inspectorで指定

    public AudioClip BGM_scenes;//BGM、AudioClip型変数BGM_scenesを宣言

    /// <summary>
    /// 最初に行う。カウンタリセットとゲーム難易度判定
    /// </summary>
    void Awake()
    {
        //BGMをプレイヤーのところにプレイする
        AudioSource.PlayClipAtPoint(BGM_scenes, GameObject.Find("slime").transform.position, 109.47f);

        TimeValue = startTimeCount;// タイムカウンターをリセット
        LifeValue = startLifeCount;// 命カウンターをリセット
        TargetValue = startTargetCount;// ターゲットカウンターをリセット
        GameManager.over = false; //ゲームの状態overをリセット


        //GameManager.playLeve = 4;//難易度テストに使う


        if (GameManager.playLeve == 0)// もし、初プレイだったら
        {
            GameManager.start = false;//すぐにスタートしない、チュートリアル起動
            //チュートリアル用鮭寿司を表示
            GameObject.Find("sushi").transform.Find("red (4)").gameObject.SetActive(true);
            TimeValue += 20;// タイムカウンターを20秒増やす
        }
        else//じゃなかった場合
        {
            GameManager.start = true; //操作可能のスタートにする
            //UIの最終ターゲット用マグロ寿司を表示
            GameObject.Find("Canvas").transform.Find("target").Find("Image").gameObject.SetActive(true);

            if (GameManager.playLeve >= 2)// もし、一回クリアしたら
            {
                //この二つのわさび寿司を削除
                Destroy(GameObject.Find("green (14)"));
                Destroy(GameObject.Find("green (10)"));
            }
            if (GameManager.playLeve >= 3)// もし、二回クリアしたら
            {
                StartCoroutine(SushiDown());// わさび寿司雨をスタート
            }
        }
    }

    /// <summary>
    /// タイムカウンターダウンをずっと行う。
    /// </summary>
    void Update()
    {
        if (GameManager.start == true)//スタートから行う
        {
            if (TimeValue > 0)//もし、タイムカウンターは0以上の場合
            {
                TimeValue -= Time.deltaTime; //タイムカウンターダウンして
                if (TimeValue < 0)//もし、タイムカウンターは0以下の場合
                {
                    TimeValue = 0;//タイムカウンターを0に表示
                }
            }
        }
    }

    /// <summary>
    /// 空からわさび寿司が落ちて来る
    /// </summary>
    /// <returns></returns>
    IEnumerator SushiDown()
    {
        while (GameManager.start == true && GameManager.over == false)//プレイ状態の時だったらずっと行う。
        {
            yield return new WaitForSeconds(3f);//3秒ごとに
            //落ちて来る高さ、float型変数yを宣言し、プレイヤーの上に15ピクセルに設定
            float y = GameObject.Find("slime").transform.position.y + 15;
            if (y > 42)//もし、落ちて来る高さは42以上の場合
            {
                y = 42;//42に設定
            }
            //Resourcesのわさび寿司Prefabをゲーム実体化し、左右は三つにランダムに設定、奥の距離は3に設定
            Instantiate(Resources.Load("greenDown"), new Vector3(Random.Range(-1, 2), y, 3), transform.rotation);
        }
    }
}
