using UnityEngine;

/// <summary>
/// プレイヤーに追加、画面をタッチすると、ジャンプする 
/// </summary>
public class TuchJump : MonoBehaviour 
{
	public float speed = 20; // スピード：Inspectorで指定
    public AudioClip jump;//ジャンプする時の音
    public float vx = 0;//左右にジャンプする移動量
    public float vy = 0;//上にジャンプする移動量

    Rigidbody2D rbody; //Rigidbody2D型変数rbodyを宣言
    int Staytimes=0;//タッチの瞬間に指先と画面接触のフレーム数
    bool leftFlag = false;//左向き可否を決めるbool型変数を宣言
    Vector2 tuchPos;//タッチポジション、Vector2型変数tuchPosを宣言

    /// <summary>
    /// 最初に行う。重力を10にして、衝突時にRigidbody2Dの回転をさせない
    /// </summary>
    void Start () 
    { 
		rbody = GetComponent<Rigidbody2D>();//コンポネントRigidbody2Dを取得
        rbody.gravityScale = 10;
		rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

    /// <summary>
    /// ずっと行う。タッチポジションによってジャンプする移動量を入れる
    /// </summary>
    void Update ()
    {
        if (GameManager.start == true)//スタートから移動量を0にリセット
        {
            vx = 0;
            vy = 0;
        }

        if (GameManager.start == true&& GameManager.over == false)//プレイ状態の時にタッチの動作をチェック
        {
            if (Input.GetMouseButtonDown(0))// もし、タッチがスタートしたら
            {
                //ジャンプの音をカメラのところにプレイする
                AudioSource.PlayClipAtPoint(jump, Camera.main.transform.position, 0.273f);
                tuchPos = Input.mousePosition;//タッチポジションをゲット
                 //タッチポジションをオブジェクトとポジション比べ可能なワルド座標に変える
                tuchPos = Camera.main.ScreenToWorldPoint(tuchPos);
                if (tuchPos.x > transform.position.x) //もし、タッチポジションはプレイヤーの右側の場合
                {
                    leftFlag = false;//左向き否
                }
                else//じゃなかった場合
                {
                    leftFlag = true;//左向き否
                }
            }


            if (Input.GetMouseButtonUp(0))// もし、タッチが終わったら
            {
                Staytimes = 0;//タッチのフレーム数を0にリセット
            }


            if (Input.GetMouseButton(0)&& Staytimes < 10) //もし、指先と画面接触しているフレーム数は10回以内の場合
            {
                Staytimes += 1;//タッチのフレーム数に1を追加
                vy += speed; // 上にジャンプす移動量を入れる
                // 左右にジャンプす移動量に設定スピードの三分の一を入れる
                vx = speed/3 * (tuchPos.x - transform.position.x); 
            }
        }
	}
    /// <summary>
    /// ずっと行う（一定時間ごとに）
    /// </summary>
	void FixedUpdate() 
    {
		rbody.velocity = new Vector2(vx, vy); // ジャンプする
        GetComponent<SpriteRenderer>().flipX = leftFlag;// 左右の向きを変える
    }
}
