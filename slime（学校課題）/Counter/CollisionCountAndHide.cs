using UnityEngine;

/// <summary>
/// 寿司に追加、衝突すると、カウントを増やして自分を消す
/// </summary>
public class CollisionCountAndHide : MonoBehaviour 
{
	public string targetObjectName; // 目標オブジェクト名：Inspectorで指定
	public int addValue = 1;    // 増加量：Inspectorで指定

    float colTime;//衝突時間float型変数colTimeを宣言
    bool destroy;//削除中のbool型変数destroyを宣言

    /// <summary>
    /// 衝突したときの変化
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay2D(Collision2D collision)
    {
        // もし、衝突可能の状態で衝突した物の名前が目標オブジェクトだったら
        if (collision.gameObject.name == targetObjectName&&GameManager.over == false&& destroy==false)
        {
            //Vector3型変数sushiScaleを宣言し寿司の体積に設定
            Vector3 sushiScale = transform.localScale;
            //衝突時間で記録、寿司の体積による溶解スピードが違ってくる。
            colTime += Time.deltaTime / Mathf.Pow((sushiScale.x + sushiScale.y + sushiScale.z) / 3, 3) * 4f;
            //溶解を色変化で表示
            GetComponent<SpriteRenderer>().color = new Color(1 - colTime, 1, 1);

            // もし、溶解完了、真っ青になった場合
            if (colTime >= 1f)
            {
                if(name== "pink")// もし、衝突したもの名前がマグロ寿司の名前だったら
                {
                    GameCounter.TargetValue += addValue;// ターゲットカウンターの値を増やす
                    //チュートリアルのセリフarigatouを表示
                    GameObject.Find("Canvas").transform.Find("arigatou").gameObject.SetActive(true);
                }
                else// もし、衝突したもの名前がマグロ寿司の名前じゃなかったら
                {
                    if (name == "yellow")// もし、衝突したもの名前がだし巻き寿司の名前だったら
                    {
                        GameCounter.TimeValue += addValue*10;// タイムカウンターの値を増やす
                        //タイムカウンターのupアニメをプレイする
                        GameObject.Find("TimeCount").GetComponent<Animator>().SetTrigger("up");

                        if (GameManager.playLeve==0)// もし、初プレイだったら
                        {
                            //もし、チュートリアルのセリフkanro存在
                            if (GameObject.Find("Canvas").transform.Find("kanro") != null)
                            {
                                //チュートリアルの突出しkanroを表示
                                GameObject.Find("Canvas").transform.Find("kanro").gameObject.SetActive(true);
                                //チュートリアルのセリフkanrowardsを表示
                                GameObject.Find("Canvas").transform.Find("kanrowards").gameObject.SetActive(true);

                                // もし、プレイヤーが画面の右側だったら
                                if (GameObject.Find("slime").transform.position.x > 0)
                                {
                                    //突出しkanroのポジションをプレイヤーの右上に設定
                                    GameObject.Find("Canvas").transform.Find("kanro").GetComponent<RectTransform>().anchoredPosition = new Vector3(311, Camera.main.WorldToScreenPoint(GameObject.Find("slime").transform.position).y + 300, 0);
                                    //セリフkanrowardsのポジションをプレイヤーの右上に設定
                                    GameObject.Find("Canvas").transform.Find("kanrowards").GetComponent<RectTransform>().anchoredPosition = new Vector3(311 + 29, Camera.main.WorldToScreenPoint(GameObject.Find("slime").transform.position).y + 312, 0);
                                }
                                else//じゃなかった場合
                                {
                                    //突出しkanroのポジションをプレイヤーの左上に設定
                                    GameObject.Find("Canvas").transform.Find("kanro").GetComponent<RectTransform>().anchoredPosition = new Vector3(-291, Camera.main.WorldToScreenPoint(GameObject.Find("slime").transform.position).y + 300, 0);
                                    //突出しkanroを左向きに設定
                                    GameObject.Find("Canvas").transform.Find("kanro").transform.localScale = new Vector3(-1, 1, 1);
                                    //セリフkanrowardsのポジションをプレイヤーの左上に設定
                                    GameObject.Find("Canvas").transform.Find("kanrowards").GetComponent<RectTransform>().anchoredPosition = new Vector3(-291 + 29, Camera.main.WorldToScreenPoint(GameObject.Find("slime").transform.position).y + 312, 0);
                                }
                            }
                        }
                    }
                    // もし、衝突したもの名前がマグロ寿司の名前じゃなかったら、鮭寿司とわさび寿司の場合
                    else
                    {
                        GameCounter.LifeValue += addValue;// 命カウンターの値を変える

                        if (addValue > 0)// もし、鮭寿司は鮭寿司の場合
                        {
                            //命カウンターのupアニメをプレイする
                            GameObject.Find("LifeCount").GetComponent<Animator>().SetTrigger("up");

                            // もし、チュートリアル中だったら
                            if (GameManager.start == false && GameManager.over == false)
                            {
                                //チュートリアルの突出しumaを表示
                                GameObject.Find("Canvas").transform.Find("uma").gameObject.SetActive(true);
                            }
                        }
                        else//じゃなかった場合
                        {
                            //命カウンターのdownアニメをプレイする
                            GameObject.Find("LifeCount").GetComponent<Animator>().SetTrigger("down");

                            // もし、チュートリアル中だったら
                            if (GameManager.start == false && GameManager.over == false)
                            {
                                //チュートリアルの突出しshinisouを表示
                                GameObject.Find("Canvas").transform.Find("shinisou").gameObject.SetActive(true);

                                //プレイヤーのジャンプをストップする
                                GameObject.Find("slime").GetComponent<TuchJump>().vy = 0;
                                GameObject.Find("slime").GetComponent<TuchJump>().vx = 0;

                                GameManager.start = true;//操作可能のスタートにする
                            }
                        }
                    }
                }
                
                Destroy(gameObject,0.5f);// 0.5秒後自分自身を削除
                destroy = true;// 削除中に設定
                Destroy(GetComponent<MoveCollisionFlip>());// 移動スクリプトを削除
            }
		}
	}

    /// <summary>
    /// ずっと行う。削除中の変化
    /// </summary>
    void Update()
    {
        if(destroy == true) // もし、削除中だったら
        {
            // 体積を縮小
            transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime)*2;
            // 透明度を増加
            GetComponent<SpriteRenderer>().color = new Color(Color.white.r, Color.white.g, Color.white.b, GetComponent<SpriteRenderer>().color.a - Time.deltaTime * 2);
        }
    }
}
