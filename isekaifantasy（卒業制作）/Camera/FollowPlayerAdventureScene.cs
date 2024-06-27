using System.Collections;
using UnityEngine;

/// <summary>
/// カメラに追加し、固定視点でプレイヤーと一定の距離を保て上下で移動する
/// </summary>
public class FollowPlayerAdventureScene : CameraBace
{
    //private float pix=11.2f;
    protected float xMin;
    protected float xMax;

    private void Start()
    {
        trs = transform;
        GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
    }
    public void Init()
    {
        if (PlayerManager.Instance.Hero == null) return;
        trs.position = PlayerManager.Instance.Hero.trs.position+new Vector3(0,5,-3f);
    }
    public void InRoom()
    {
        zMin = AdSceneManager.Instance.CameraZMinRoom + AdModeManager.Instance.nowRoom.trs.position.z;
        zMax = AdSceneManager.Instance.CameraZMaxRoom + AdModeManager.Instance.nowRoom.trs.position.z;
        xMin = AdSceneManager.Instance.CameraXMinRoom + AdModeManager.Instance.nowRoom.trs.position.x;
        xMax = AdSceneManager.Instance.CameraXMaxRoom + AdModeManager.Instance.nowRoom.trs.position.x;
    }

    public void InMap()
    {
        zMin = AdSceneManager.Instance.CameraZMinMap;
        zMax = AdSceneManager.Instance.CameraZMaxMap;
        xMin = AdSceneManager.Instance.CameraXMinMap;
        xMax = AdSceneManager.Instance.CameraXMaxMap;
    }

    protected virtual void LateUpdate()
    {
        if (PlayerManager.Instance.Hero == null) return;
        var z = PlayerManager.Instance.Hero.trs.position.z - 15f;
        var x = PlayerManager.Instance.Hero.trs.position.x;
        var y = 20;
        if (z > zMax) z = zMax;
        else if (z < zMin) z = zMin;

        if (x > xMax) x = xMax;
        else if (x < xMin) x = xMin;

        targetPos = new Vector3(x, y, z);
        trs.position = Vector3.Lerp(trs.position, targetPos, speed * Time.deltaTime);
    }
}
