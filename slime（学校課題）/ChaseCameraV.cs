using UnityEngine;

/// <summary>
/// プレイヤーに追加、カメラをずっと自分と一定の距離を保つ
/// </summary>
public class ChaseCameraV : MonoBehaviour {

    public float speed = 2.0f;// スピード：Inspectorで指定

    /// <summary>
    /// ずっと行う毎回Updateの後に行う。カメラを自分と一定の距離を保つ
    /// </summary>
    void LateUpdate () {
        float interpolation = speed * Time.deltaTime;//float型変数interpolationを宣言、スピードの一フレームの分量

        Vector3 pos = transform.position;//Vector3型変数posを宣言し先に自分のポジションに設定しておく
        pos.z = 1;//奥の距離を1に一番手前に設定
        pos.x = 0;//左右の距離を0に固定

        if (pos.y<=0)//もし、上下の距離が0になった場合
        {
            pos.y = 0;//上下の距離を0に設定
        }
        else//じゃなかった場合
        {
            //上下の距離をinterpolationを使って、固定フレームレートで自分の高さになめらかに設定
            pos.y = Mathf.Lerp(Camera.main.gameObject.transform.position.y, transform.position.y, interpolation);
        }

        Camera.main.gameObject.transform.position = pos;//カメラポジションをposに設定
    }
}
