using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音声認識でのスキルの使用、ダウンロードしたアンドロイドパッケージを
/// 使用して、書き直した機能です
/// </summary>
public class SpeechRecognizerTest : MonoBehaviour {
    /*
    public GameObject receiveObject;
    public Text displayText;
    public Animator circleAnimator;

    Joystick hdl;
    string ss;
    string ss1;
    string ss2;
    string ss3;
    string ss4;
    string ss5;
  
    /// <summary>
    /// スタートから音声認識開始
    /// </summary>
    private void Start () {
        if (receiveObject == null)
            receiveObject = gameObject;

        if (displayText != null)
            displayText.text = "keywards:\nファイアーボール\nエナジーバースト\nドラゴンフレーム\nエクスプロージョン\n破滅せよ、エクスプロージョン";

        StartSpeechRecognizer();
    }

    /// <summary>
    /// アプリケーションを消された時に機能停止
    /// </summary>
    public void OnDestroy()
    {
#if UNITY_EDITOR
        Debug.Log("AndroidPlugin.Release called");
#elif UNITY_ANDROID
        AndroidPlugin.Release();
#endif
    }

    /// <summary>
    /// 音声認識開始
    /// </summary>
    public void StartSpeechRecognizer()
    {
#if UNITY_EDITOR
        Debug.Log("StartSpeechRecognizer");
#elif UNITY_ANDROID
        AndroidPlugin.StartSpeechRecognizer(AndroidLocale.Japanese,receiveObject.name, "OnResult", "OnError", "OnReady", "OnBegin");
#endif
    }

    /// <summary>
    /// Uiで音声認識準備状態を表示
    /// </summary>
    private void OnReady(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnReady");
#endif
        if (circleAnimator != null)
            circleAnimator.SetTrigger("ready");
    }


    /// <summary>
    /// Uiで音声認識発動状態を表示
    /// </summary>
    private void OnBegin(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnBegin");
#endif
        if (circleAnimator != null)
            circleAnimator.SetTrigger("speech");
    }



    /// <summary>
    /// 音声認識成功した場合に音声の内容を表示する。音声認識を繰り返しスタート
    /// </summary>
    private void OnResult(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnResult");
#endif
        if (circleAnimator != null)
            circleAnimator.SetTrigger("stop");
        SetDisplayText(message);
        StartSpeechRecognizer();
    }


    /// <summary>
    /// 音声認識エラーになった場合に音声認識を繰り返しスタート
    /// </summary>
    private void OnError(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnError");
#endif
        if (circleAnimator != null)
            circleAnimator.SetTrigger("stop");
        StartSpeechRecognizer();
    }



    /// <summary>
    /// 音声認識を停止する
    /// </summary>
    public void StopSpeechRecognizer()
    {
#if UNITY_EDITOR
        Debug.Log("StopSpeechRecognizer");
#elif UNITY_ANDROID
        AndroidPlugin.ReleaseSpeechRecognizer();
#endif
        if (circleAnimator != null)
            circleAnimator.SetTrigger("stop");
    }

    /// <summary>
    /// 音声の内容をチェックして表示する。条件を満足した場合はスキルを放つ
    /// </summary>
    /// <param name="message"></param>
    public void SetDisplayText(string message)
    {
        if (displayText != null)
            displayText.text = message;

        string[] words = message.Split('\n');

        ss = "FIRE BALL";
        ss1 = "ファイヤー4";
        ss2 = "ファイアーボール";
        ss3 = "さやぼー";
        ss4 = "清子";
        ss5 = "ファイアボール";
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            if (s == ss || s == ss1 || s == ss2 || s == ss3 || s == ss4 || s == ss5)
            {
                displayText.text = "ファイアーボール";
                PlayerManager.Instance.HeroAni.SetTrigger("attack1-2");
                return;
            }
        }

        ss = "エナジーバースト";
        ss1 = "エナジーバスト";
        ss2 = "同じバスト";
        ss3 = "同じ場所";
        ss4 = "エナジーパスト";
        ss5 = "変な子バスト";
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            if (s == ss || s == ss1 || s == ss2 || s == ss3 || s == ss4 || s == ss5)
            {
                displayText.text = "エナジーバースト";
                hdl = Joystick.Instance.handle2;
                if (hdl.imageMp.enabled == true || hdl.imageCool.fillAmount > 0 || (hdl.imageCool.fillAmount == 0 && hdl.chargeTime != 0 && hdl.imageCharge.fillAmount != 1))
                {
                    return;
                }
                PlayerManager.Instance.HeroAni.SetTrigger("attack2");
                hdl.imageCool.fillAmount = 1;
                return;
            }
        }

        ss = "ドラゴンフレーム";
        ss1 = "ドラゴスライム";
        ss2 = "ドラゴンフライ";
        ss3 = "ドラゴンボール";
        ss4 = "ドラクエ3";
        ss5 = "ドラゴンスレ";
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            if (s == ss || s == ss1 || s == ss2 || s == ss3 || s == ss4 || s == ss5)
            {
                displayText.text = "ドラゴンフレーム";
                hdl = Joystick.Instance.handle2;
                if (hdl.imageMp.enabled == true || hdl.imageCool.fillAmount > 0 || (hdl.imageCool.fillAmount == 0 && hdl.chargeTime != 0 && hdl.imageCharge.fillAmount != 1))
                {
                    return;
                }
                PlayerManager.Instance.HeroAni.SetTrigger("attack2-2");
                hdl.imageCool.fillAmount = 1;
                return;
            }
        }

        ss = "エクスプロージョン";
        ss1 = "エグスプロージョン";
        ss2 = "エクスプローション";
        ss3 = "エグスプローション";
        ss4 = "エクスポーション";
        ss5 = "エクショ";
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            if (s == ss || s == ss1 || s == ss2 || s == ss3 || s == ss4 || s == ss5)
            {
                displayText.text = "エクスプロージョン";
                hdl = Joystick.Instance.handle3;
                if (hdl.imageMp.enabled == true || hdl.imageCool.fillAmount > 0 || (hdl.imageCool.fillAmount == 0 && hdl.chargeTime != 0 && hdl.imageCharge.fillAmount != 1))
                {
                    return;
                }
                if (hdl.gameObject.GetComponent<Animator>().enabled == true)
                {
                    hdl.gameObject.GetComponent<Animator>().enabled = false;
                    hdl.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 100 / 255f);
                    PlayerManager.Instance.HeroAni.SetTrigger("attack3-2");
                }
                else
                {
                    PlayerManager.Instance.HeroAni.SetTrigger("attack3");
                }
                hdl.imageCool.fillAmount = 1;
                return;
            }
        }

        ss = "破滅せよエクスプロージョン";
        ss1 = "破滅生エクスプロージョン";
        ss2 = "ハメセンエクスプロージョン";
        ss3 = "破滅エクスプロージョン";
        ss4 = "覇メッセエクスプロージョン";
        ss5 = "はメッセエクスプロージョン";
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            if (s == ss || s == ss1 || s == ss2 || s == ss3 || s == ss4 || s == ss5)
            {
                displayText.text = "破滅せよエクスプロージョン";
                hdl = Joystick.Instance.handle3;
                if (hdl.imageMp.enabled == true || hdl.imageCool.fillAmount > 0 || (hdl.imageCool.fillAmount == 0 && hdl.chargeTime != 0 && hdl.imageCharge.fillAmount != 1))
                {
                    return;
                }
                PlayerManager.Instance.HeroAni.SetTrigger("attack3-2");
                hdl.imageCool.fillAmount = 1;
                return;
            }
        }

        ss = "破滅せよ";
        ss1 = "破滅";
        ss2 = "破滅生";
        ss3 = "ハメセン";
        ss4 = "はメッセ";
        ss5 = "覇メッセ";
        for (int i = 0; i < words.Length; i++)
        {
            string s = words[i];
            if (s == ss || s == ss1 || s == ss2 || s == ss3 || s == ss4 || s == ss5)
            {
                displayText.text = "破滅せよ";
                hdl = Joystick.Instance.handle3;
                hdl.gameObject.GetComponent<Animator>().enabled = true;
                hdl.gameObject.GetComponent<Animator>().SetTrigger("speech");
                return;
            }
        }

        displayText.text = "ファイアーボール";
        PlayerManager.Instance.HeroAni.SetTrigger("attack1-2");
    }
    */
}
