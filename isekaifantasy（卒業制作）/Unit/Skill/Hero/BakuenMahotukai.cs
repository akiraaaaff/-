using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NpcBaceを継承し、キャラクターに追加し、BakuenMahotukaiのスキルを制御する。
/// </summary>
public class BakuenMahotukai : UnitBace
{
    private GameObject bulletPrefab;
    private List<GameObject> DamageList2;
    private List<GameObject> DamageList3;

    private bool AutoAttack;

    public void Attack()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.DamageHit);
        Transform ts = PoolManager.Instance.GetObj(bulletPrefab);
        ts.position = bulletPosLeft.position;
        ts.rotation = trs.rotation;
        if (Target != null && Target.Hp > 0)
        {
            ts.LookAt(Target.transform);
        }
        ts.localEulerAngles = new Vector3(0, trs.localEulerAngles.y, 0);
        ts.transform.localScale = new Vector3(1, 1, 1);
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        bulletBase.Init(this, target: Target);
    }
    public void Attack1_2()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.DamageHit);
        Transform ts = PoolManager.Instance.GetObj(bulletPrefab);
        ts.position = bulletPosLeft.position;
        ts.rotation = trs.rotation;
        if (Target != null && Target.Hp > 0)
        {
            ts.LookAt(Target.transform);
        }
        ts.localEulerAngles = new Vector3(0, trs.localEulerAngles.y, 0);
        ts.transform.localScale = new Vector3(3, 3, 3);
        BulletBase bulletBase = ts.GetComponent<BulletBase>();
        //bulletBase.Init(this, BulletType.aoe, AttackType.adp, attackPowerParameter: 0.5f, speed: 20f, radius: 3f, target: target);
    }
    /*
    public void Attack2()
    {
        mp -= 5;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
        Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.Firebust);
        ts.position = trs.position;

        List<UnitBace> tempList = DamageRangeManager.Instance.DamageRangeBySphere(enemyGroup, trs.position, 8f);
        for (int i = 0; i < tempList.Count; i++)
        {
            BuffBase buff = tempList[i].CheckBuffToAdd(GameManager.Instance.GameConf.buff.FirebustSparkles);
            buff.Init(BuffType.slow, tempList[i], this, 2f);
        }
    }
    */
    public void Attack2_2()
    {
        Mp -= 5;
        StartCoroutine(Attack2_2_2());
    }
    IEnumerator Attack2_2_2()
    {
        DamageList2 = new List<GameObject>();
        DamageList3 = new List<GameObject>();
        if (Target != null && Target.GetComponent<UnitBace>().Hp > 0)
        {
            transform.LookAt(Target.transform);
        }
        Vector3 pos = transform.position;
        Vector3 posForward = transform.forward;
        for (int ii = 0; ii < 11; ii++)
        {
            if (ii > 1)
            {
                yield return new WaitForSeconds(0.07f);
            }
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
            pos += posForward * ii * 0.3f;
            GameObject go = Instantiate(Resources.Load("effect/explosion"), pos, transform.rotation) as GameObject;
            go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Collider[] cols = Physics.OverlapSphere(pos, 1.5f, 1 << LayerMask.NameToLayer("Enemy") | 1 << 12);
            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        if (cols[i].gameObject.activeSelf == true && cols[i].GetComponent<UnitBace>().Hp > 0)
                        {
                            if (DamageList2.Contains(cols[i].gameObject) == false && DamageList3.Contains(cols[i].gameObject) == false)
                            {
                                cols[i].GetComponent<UnitBace>().TakeDamage(6 + magic * 0.5f, this, DamageType.magic);
                                DamageList2.Add(cols[i].gameObject);
                            }
                            else
                            {
                                if (DamageList3.Contains(cols[i].gameObject) == false)
                                {
                                    cols[i].GetComponent<UnitBace>().TakeDamage(3 + magic * 0.3f, this);
                                    DamageList3.Add(cols[i].gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void Attack3()
    {
        Mp -= 10;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
        Vector3 pos;
        if (Target != null && Target.GetComponent<UnitBace>().Hp > 0)
        {
            pos = Target.transform.position;
        }
        else
        {
            pos = transform.position + transform.forward * 5;
        }
        Instantiate(Resources.Load("effect/explosion"), pos + Vector3.up, transform.rotation);
        Collider[] cols = Physics.OverlapSphere(pos, 5f, 1 << LayerMask.NameToLayer("Enemy") | 1 << 12);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    if (cols[i].gameObject.activeSelf == true && cols[i].GetComponent<UnitBace>().Hp > 0)
                    {
                        cols[i].GetComponent<UnitBace>().TakeDamage(12 + magic, this, DamageType.magic);
                    }
                }
            }
        }
    }
    public void Attack3_2()
    {
        Mp -= 10;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicBurst);
        Vector3 pos;
        if (Target != null && Target.GetComponent<UnitBace>().Hp > 0)
        {
            pos = Target.transform.position;
        }
        else
        {
            pos = transform.position + transform.forward * 5;
        }
        GameObject go = Instantiate(Resources.Load("effect/explosion"), pos + Vector3.up, transform.rotation) as GameObject;
        go.transform.localScale = new Vector3(14, 14, 14);
        Collider[] cols = Physics.OverlapSphere(pos, 10f, 1 << LayerMask.NameToLayer("Enemy") | 1 << 12);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    if (cols[i].gameObject.activeSelf == true && cols[i].GetComponent<UnitBace>().Hp > 0)
                    {
                        cols[i].GetComponent<UnitBace>().TakeDamage(36 + magic, this, DamageType.magic);
                    }
                }
            }
        }
    }
    //public void FixedUpdate()
    //{
    //    target = SetTarget();
    //    //Pz
    //    if (Joystick.Instance != null && GameManager.mode != 1 && Joystick.Instance.handle1.imageSelect != null)
    //    {
    //        AutoAttack = Joystick.Instance.handle1.imageSelect.enabled;
    //    }
    //    if (target == null || target.GetComponent<UnitBace>().hp <= 0 || target.GetComponent<CapsuleCollider>() == null || AutoAttack == false)
    //    {
    //        anim.SetBool("attack", false);
    //        return;
    //    }

    //    float dis = Vector3.Distance(transform.position, target.transform.position) - target.GetComponent<CapsuleCollider>().radius;
    //    if (dis < attackDistance)
    //    {
    //        anim.SetBool("attack", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("attack", false);
    //    }
    //}
}
