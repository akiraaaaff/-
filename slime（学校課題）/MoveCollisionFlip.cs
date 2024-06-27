using UnityEngine;

/// <summary>
/// 寿司に追加、ずっとゲーム難易度による移動して、水平に衝突すると反転する
/// </summary>
public class MoveCollisionFlip : MonoBehaviour 
{
	public float speedH; // スピード水平：Inspectorで指定
    public float speedV; // スピード垂直：Inspectorで指定

    Rigidbody2D rbody;//Rigidbody2D型変数rbodyを宣言

    /// <summary>
    /// 最初に行う。ゲーム難易度による移動の初期化
    /// </summary>
    void Start () 
    {
		rbody = GetComponent<Rigidbody2D>();//コンポネントRigidbody2Dを取得
        rbody.gravityScale = 0;// 重力を0にして

        // もし、一回クリアして自分はマグロ寿司だったか三回クリアした場合に行う
        if (GameManager.playLeve >= 2&&name== "pink"|| GameManager.playLeve > 3)
        {
            rbody.constraints = ~RigidbodyConstraints2D.FreezePosition; //衝突時にRigidbody2Dの移動以外をさせない
            speedV = speedH; //スピード垂直をスピード水平と同じ量に設定
        }
        else//じゃなかった場合
        {
            rbody.constraints = ~RigidbodyConstraints2D.FreezePositionX;//衝突時にRigidbody2Dの左右移動以外をさせない
        }

        int speedRandom = Random.Range(-1, 1);//ランダム値、int型変数speedRandomを宣言し、範囲は-1と0に設定
        if (speedRandom < 0) //もし、0以下だったら
        {
            speedH = -speedH;//スピード水平反転
        }
        speedRandom = Random.Range(-1, 1);//もう一回行う
        if (speedRandom < 0)
        {
            speedV = -speedV;//スピード垂直
        }
    }

    /// <summary>
    /// ずっと行う（一定時間ごとに）。移動と水平衝突反転
    /// </summary>
    void FixedUpdate() 
    { 
        Vector3 pos = transform.position;//Vector3型変数posを宣言し先に自分のポジションに設定しておく

        if (pos.x < -2)//もし、左右の値が-2以下だったら
        {
            speedH = Mathf.Abs(speedH); // 水平進む向きを反転する
            GetComponent<SpriteRenderer>().flipX = (speedH < 0);// 進む向きで左右の向きを変える
        }

        if (pos.x > 2)//もし、左右の値が2以上だったら
        {
            speedH = -Mathf.Abs(speedH); // 水平進む向きを反転する
            GetComponent<SpriteRenderer>().flipX = (speedH < 0);// 進む向きで左右の向きを変える
        }

        if (pos.y < 17&& name == "pink")//もし、高さが17以下で自分はマグロ寿司だったら
        {
            speedV = Mathf.Abs(speedH); // 垂直進む向きを反転する
        }

        rbody.velocity = new Vector2(speedH, speedV);// Rigidbody2Dを使って移動
    }

    /// <summary>
    /// 衝突したとき反転
    /// </summary>
	void OnCollisionEnter2D() 
    { 
        speedH = -speedH;// 水平進む向きを反転する
        speedV = -speedV;// 垂直進む向きを反転する
        GetComponent<SpriteRenderer>().flipX = (speedH < 0);// 進む向きで左右の向きを変える
    }
}
