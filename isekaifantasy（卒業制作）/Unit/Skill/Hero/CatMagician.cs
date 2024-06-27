using System.Collections;
using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、CatMagicianのスキルを制御する。
/// </summary>
public class CatMagician : UnitBace
{
    public AudioClip attackClip;
    Vector3 position;
    GameObject coin;

    public void Attack()
    {
        if (anim.GetBool("run") == false)
        {
            AudioSource.PlayClipAtPoint(attackClip, transform.position, 0.47f);
            if (Target != null && Target.GetComponent<UnitBace>().Hp > 0)
            {
                float distance = Vector3.Distance(transform.position, Target.transform.position) - Target.GetComponent<CapsuleCollider>().radius;
                if (distance <= attackMissDistance)
                {
                    Target.GetComponent<UnitBace>().TakeDamage(attack, this, DamageType.attack);
                }
            }
        }

    }
    public void Attack2()
    {
        Mp -= 6;
    }
    public void Attack2_2()
    {
        AudioSource.PlayClipAtPoint(attackClip, transform.position, 0.47f);
        float angle = 90f;
        for (int i = 0; i < 7; i++)
        {
            position = transform.position;
            coinAdd();
            coin.transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + angle, 0);
            coin.transform.Translate(Vector3.forward * 1);
            angle -= 30f;
        }
        if (magic > 0)
        {
            for (int i = 0; i < magic / 4; i++)
            {
                position = transform.position + Random.insideUnitSphere * 1;
                coinAdd();
            }
        }
    }
    public void Attack2_3()
    {
        //GameManager.coinBoom += 60;
    }
    public void Attack3()
    {
        AudioSource.PlayClipAtPoint(attackClip, transform.position, 0.47f);
        position = transform.position;
        coinAdd();
        StartCoroutine(coinWaitBoom(coin, 0.5f));


        transform.position += transform.forward * 5;



        position = transform.position;
        coinAdd();
        StartCoroutine(coinWaitBoom(coin, 1.5f));
    }
    //public void FixedUpdate()
    //{
    //    target = SetTarget();
    //    if (boomtime > 0)
    //    {
    //        //GameManager.coinBoom -= 1;
    //        boomtime -= 1;
    //    }
    //    if (hp == 0)
    //    {
    //        //GameManager.coinBoom = 0;
    //        boomtime = 0;
    //    }
    //    //Pz
    //    if (Joystick.Instance != null && GameManager.mode != 1 && Joystick.Instance.handle1.imageSelect != null)
    //    {
    //        //AutoAttack = Joystick.Instance.imageSelect.enabled;
    //    }

    //    if (target == null || target.GetComponent<UnitBace>().hp <= 0 || target.GetComponent<CapsuleCollider>() == null || anim.GetBool("run") == true || AutoAttack == false)
    //    {
    //        anim.SetBool("attack", false);
    //        return;
    //    }

    //    float distance = Vector3.Distance(transform.position, target.transform.position) - target.GetComponent<CapsuleCollider>().radius;
    //    if (distance < attackDistance)
    //    {
    //        anim.SetBool("attack", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("attack", false);
    //    }
    //}
    void coinAdd()
    {
        coin = Instantiate(Resources.Load("Gold"), new Vector3(position.x, 0.8f, position.z), transform.rotation) as GameObject;
        coin.GetComponent<SphereCollider>().radius = 0.1f;
        coin.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

        //Pz
        //if (GameManager.mode != 1)
        //{
        //    coin.GetComponent<Coin>().count = 2f;
        //}
        //else
        //{
        //    coin.GetComponent<Coin>().count = 500f;
        //}
    }
    IEnumerator coinWaitBoom(GameObject coin, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (coin != null)
        {
            coin.GetComponent<Coin>().creatEff();
        }
    }
}
