using System.Collections.Generic;
using UnityEngine;

public class CanBreakItems : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private int maxhp = 5;
    [SerializeField]
    private Color shatterColor = Color.white;

    private List<Material> materialList = new List<Material>();
    [SerializeField]
    private List<GameObject> materObjList = new List<GameObject>();

    [SerializeField]
    private GameObject parent;
    private GameObject obj;
    private Transform trs;

    private int hp;

    private void Awake()
    {
        obj = gameObject;
        obj.layer = 10;
        trs = transform;
        tag = Tags.canBreakObj.ToString();
        hp = maxhp;
        for (int i = 0; i < materObjList.Count; i++)
            materialList.Add(materObjList[i].GetComponent<Renderer>().sharedMaterial);
    }

    public void TakeDamage(int damage, Transform tarTrs)
    {
        hp -= damage;
        if (hp <= 0)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
            Transform ts = PoolManager.Instance.GetObj
                (GameManager.Instance.GameConf.h_Effects.h_Hit_block);
            ts.position = trs.position;
            ts.LookAt(tarTrs);
            var rota = ts.localEulerAngles;
            ts.localEulerAngles = new Vector3(rota.x, rota.y - 180f, rota.z);

            var particle = ts.GetComponent<ParticleSystem>();
            var main = particle.main;
            main.startColor = shatterColor;
            DestroyPar();
        }
        else
        {
            SetMaterialHit();
        }
    }

    private void DestroyPar() => Destroy(parent);

    private void SetMaterialHit()
    {
        for (int i = 0; i < materObjList.Count; i++)
        {
            Material material = materObjList[i].GetComponent<Renderer>().material;
            material.SetFloat("_RimSize", 0);
        }
        Invoke(nameof(ResetMaterialHit), 0.3f);
    }
    private void ResetMaterialHit()
    {
        for (int i = 0; i < materObjList.Count; i++)
        {
            Material material = materObjList[i].GetComponent<Renderer>().material;
            material.SetFloat("_RimSize", 1);
        }
    }
}
