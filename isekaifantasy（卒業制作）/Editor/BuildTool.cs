using UnityEngine;

public class BuildTool : MonoBehaviour
{
    [SerializeField, Range(0.1f, 3f)]
    private float posRange = 2f;

    [SerializeField, Range(0.01f, 1f)]
    private float scaleRange = 0.05f;


    void Reset()
    {
        var trs = transform;
        var pos = trs.position;
        var rX = Random.Range(-posRange, posRange);
        var rZ = Random.Range(-posRange, posRange);
        trs.position = new Vector3(pos.x + rX, pos.y, pos.z + rZ);


        var ro = trs.eulerAngles;
        var rR = Random.Range(-180, 180);
        trs.rotation = Quaternion.Euler(ro.x, ro.y + rR, ro.z);
    }
    public void RandomSet()
    {
        var trs = transform;
        var pos = trs.position;
        var rX = Random.Range(-posRange, posRange);
        var rZ = Random.Range(-posRange, posRange);
        trs.position = new Vector3(pos.x + rX, pos.y, pos.z + rZ);


        var ro = trs.eulerAngles;
        var rR = Random.Range(-180, 180);
        trs.rotation = Quaternion.Euler(ro.x, ro.y + rR, ro.z);


        var sc = trs.localScale;
        var sR = scaleRange;
        var rS = Random.Range(0, 12);
        switch (rS)
        {
            case 0:
                trs.localScale = new Vector3(sc.x + sR, sc.y, sc.z);
                break;
            case 1:
                trs.localScale = new Vector3(sc.x + sR, sc.y + sR, sc.z);
                break;
            case 2:
                trs.localScale = new Vector3(sc.x + sR, sc.y + sR, sc.z + sR);
                break;
            case 3:
                trs.localScale = new Vector3(sc.x, sc.y + sR, sc.z);
                break;
            case 4:
                trs.localScale = new Vector3(sc.x, sc.y + sR, sc.z + sR);
                break;
            case 5:
                trs.localScale = new Vector3(sc.x, sc.y, sc.z + sR);
                break;
            case 6:
                trs.localScale = new Vector3(sc.x - sR, sc.y, sc.z);
                break;
            case 7:
                trs.localScale = new Vector3(sc.x - sR, sc.y - sR, sc.z);
                break;
            case 8:
                trs.localScale = new Vector3(sc.x - sR, sc.y - sR, sc.z - sR);
                break;
            case 9:
                trs.localScale = new Vector3(sc.x, sc.y - sR, sc.z);
                break;
            case 10:
                trs.localScale = new Vector3(sc.x, sc.y - sR, sc.z - sR);
                break;
            case 11:
                trs.localScale = new Vector3(sc.x, sc.y, sc.z - sR);
                break;
        }
    }
    public void HalfSet()
    {
        var trs = transform;


        var sc = trs.localScale;
        var sR = -0.3f;
        trs.localScale = new Vector3(sc.x + sR, sc.y + sR, sc.z + sR);
    }
}
