using UnityEngine;

/// <summary>
/// Coinに追加、存在時間、増加Coin量、爆発などを制御
/// </summary>
public class Coin : MonoBehaviour
{
    public AudioClip CoinsClip;
    public GameObject coinsEff;
    public float count;

    float time;
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(CoinsClip, transform.position, 1.202f);
    }
    void FixedUpdate()
    {
        //time += Time.deltaTime;
        ////Pz
        //if (GameManager.mode == 1)
        //{
        //    if (time >= 12)
        //    {
        //        if (GetComponent<Light>() != null)
        //        {
        //            Destroy(GetComponent<Light>());
        //        }
        //    }
        //    if (time >= 15)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        //else
        //{
        //    if (transform.position.y > 0.5f)
        //    {
        //        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        //    }

        //    if (time >= 5)
        //    {
        //        creatEff();
        //    }
        //}
        //if (time >= 0.3f)
        //{
        //    creatEff();
        //}
    }
    public void creatEff()
    {
        Instantiate(coinsEff, transform.position + Vector3.up, transform.rotation);
        AudioSource.PlayClipAtPoint(CoinsClip, transform.position, 1.202f);

        //if (PlayerManager.Instance.Hero.obj==GameManager.Instance.GameConf.heroList[(int)NpcName.CatMagician])
        //{
        //    if (GameManager.Instance.enemyList.Count > 0)
        //    {
        //        count *= 3;
        //        if (count > 30)
        //        {
        //            count = 30;
        //        }
        //        for (int i = 0; i < GameManager.Instance.enemyList.Count; i++)
        //        {
        //            float distance = Vector3.Distance(transform.position, GameManager.Instance.enemyList[i].transform.position);
        //            if (distance<1.5f && GameManager.Instance.enemyList[i].GetComponent<UnitBace>().hp > 0)
        //            {
        //                GameManager.Instance.enemyList[i].GetComponent<UnitBace>().TakeDamage(count, PlayerManager.Instance.Hero, DamageType.no);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    if (GameManager.Instance.friendsList.Count>0)
        //    {
        //        for (int ii = 0; ii < GameManager.Instance.friendsList.Count; ii++)
        //        {
        //            if (GameManager.Instance.friendsList[ii].gameObject.activeSelf == true && GameManager.Instance.friendsList[ii].GetComponent<UnitBace>().hp > 0 && GameManager.Instance.friendsList[ii].name == "CatMagician")
        //            {
        //                if (GameManager.Instance.enemyList.Count > 0)
        //                {
        //                    count *= 3;
        //                    if (count > 30)
        //                    {
        //                        count = 30;
        //                    }
        //                    for (int i = 0; i < GameManager.Instance.enemyList.Count; i++)
        //                    {
        //                        float distance = Vector3.Distance(transform.position, GameManager.Instance.enemyList[i].transform.position);
        //                        if (distance < 1.5f && GameManager.Instance.enemyList[i].GetComponent<UnitBace>().hp > 0)
        //                        {
        //                            GameManager.Instance.enemyList[i].GetComponent<UnitBace>().TakeDamage(count, GameManager.Instance.friendsList[ii].GetComponent<UnitBace>(), DamageType.no);
        //                        }
        //                    }
        //                }
        //                break;
        //            }
        //        }
        //    }
        //}
        Destroy(gameObject);
    }
}
