using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectsBase : MonoBehaviour
{
    private UnitBace owner;
    private UnitBace target;
    [HideInInspector] public SkillPara skillPara;
    private float attack;
    private DamageType damageType;
    private GameObject buffAdd;
    private float buffTime;
    private float buffValue;
    private BuffType buffType;
    private DamageRange range;
    private float rangeInnerRadius;
    private float rangeHeight;
    private Collider col;
    protected float loopTime;
    private GameObject h_Prefab;
    private GameObject o_Prefab;
    private AudioClip clip;
    private float waitTime;

    private List<Collider> executedList;
    private List<Collider> onceExecutedList;

    [SerializeField]
    protected List<ParticleSystem> colorParticleList;
    protected List<ParticleSystem.MainModule> colorMainList;
    [HideInInspector] public Color defaultColor;
    [HideInInspector] public Color color = default;

    [SerializeField, Range(0.1f, 10f)]
    protected float destroyTime = 1;
    [SerializeField, Range(0.1f, 10f)]
    protected float disAbleTime;
    [SerializeField, Range(0f, 10f)]
    protected float ableTime;
    protected bool isAble;
    protected Transform trs;
    protected GameObject obj;
    private bool isHit;

    [HideInInspector] protected Func<UnitBace, UnitBace, ReSetDamage> reSetDamage;



    protected virtual void Awake()
    {
        trs = transform;
        obj = gameObject;
        executedList = new List<Collider>();
        onceExecutedList = new List<Collider>();
        if (disAbleTime == 0) disAbleTime = destroyTime * 0.5f;

        if (colorParticleList != null && colorParticleList.Count > 0)
        {
            defaultColor = colorParticleList[0].main.startColor.color;
            colorMainList = new List<ParticleSystem.MainModule>();
            for (var i = 0; i < colorParticleList.Count; i++)
                colorMainList.Add(colorParticleList[i].main);
        }
    }

    private void OnEnable()
    {
        Able();
    }
    private void Able()
    {
        CancelInvoke("DestroyByPool");
        CancelInvoke("DisAble");
        if (ableTime > 0)
            Invoke("On", ableTime);
        else On();
    }

    private void WaitToStart()
    {
        obj.SetActive(true);
        Able();
    }

    protected virtual void On()
    {
        isAble = true;


        if (target != null) TakeEffects(target.col);

        if (destroyTime > 0)
        {
            Invoke("DestroyByPool", destroyTime);
            Invoke("DisAble", disAbleTime);
        }
        RefreshCollider();
        if (clip != null)
            AudioManager.Instance.PlayEFAudio(clip);
    }
    private void DisAble()
    {
        isAble = false;
        if (!isHit)
        {
            if ((damageType == DamageType.attack || damageType == DamageType.magicArrow) && owner != null)
                owner.attackAfterShockEffects?.Invoke(trs);
            if (skillPara != null)
                skillPara.damageEffects?.Invoke(target, trs);
        }
    }

    public virtual void DestroyByPool()
    {
        if (o_Prefab != null)
        {
            Transform ts = PoolManager.Instance.GetObj(o_Prefab);
            ts.position = trs.position;
            ts.rotation = trs.rotation;
            if (color != default)
            {
                EffectsBase eff = ts.GetComponent<EffectsBase>();
                eff.Init(color: color);
            }
        }

        CancelInvoke();
        ReSet();
        obj.SetActive(false);
        PoolManager.Instance.PushObj(trs);
    }
    public void ReSet()
    {
        isAble = false;
        this.owner = null;
        this.target = null;
        this.skillPara = null;
        this.h_Prefab = null;
        this.o_Prefab = null;
        this.attack = 0;
        this.damageType = 0;
        this.buffAdd = null;
        this.buffType = 0;
        this.buffTime = 0;
        this.buffValue = 0;
        this.range = 0;
        this.rangeInnerRadius = 0;
        this.rangeHeight = 0;
        this.loopTime = 0;
        this.clip = null;
        this.waitTime = 0;
        this.isHit = false;
        reSetDamage = null;
        executedList.Clear();
        onceExecutedList.Clear();
    }

    private void OnTriggerEnter(Collider tar)
    {
        if (GameManager.Instance.canNotAtkList.Contains(tar)) return;
        if (!isAble) return;
        if (target != null) return;
        if (executedList.Contains(tar)) return;
        if (owner.enemyGroup.Contains(tar) ||
            (GameManager.Instance.friendsCanAtkList.Contains(tar) &&
            owner.col != tar) ||
            tar.tag == Tags.canBreakObj.ToString())
        {
            if (range != DamageRange.no)
            {
                //獲得Colliderの最近点
                Vector3 tarNearestRadiusPos = tar.ClosestPoint(trs.position);
                Vector3 tarPos = tar.transform.position;
                //Colliderの最近点で獲得大体の半径
                float radius = Vector3.Distance(tarPos, tarNearestRadiusPos);
                //獲得Y軸距離
                float disY = tarPos.y - trs.position.y;
                switch (range)
                {
                    case DamageRange.hemisphere:
                        //原点と直径判断最低平面距離
                        if (disY < -(0.1f + radius * 2)) return;
                        break;
                    case DamageRange.cylinder:
                        if (disY < -(0.1f + radius * 2) || disY > rangeHeight) return;
                        break;
                    case DamageRange.cone:
                        if (disY < -(0.1f + radius * 2) || disY > rangeHeight) return;
                        float disdisPlane = Vector3.Distance(new Vector3(tarPos.x, 0, tarPos.z), new Vector3(trs.position.x, 0, trs.position.z));
                        //平面距離+ターゲット半径＝外辺縁距離＜内径
                        if (disdisPlane + radius < rangeInnerRadius) return;
                        break;
                }
            }
            TakeEffects(tar);
        }
    }

    private void TakeEffects(Collider tar)
    {
        isHit = true;
        executedList.Add(tar);
        Transform tarTrs;
        if (tar.tag == Tags.canBreakObj.ToString())
        {
            if (onceExecutedList.Contains(tar))
                return;
            if (damageType != DamageType.attack &&
                damageType != DamageType.magic)
                return;
            onceExecutedList.Add(tar);
            tar.GetComponent<CanBreakItems>().TakeDamage(1, trs);
            tarTrs = tar.transform;
        }
        else
        {
            UnitBace target = tar.GetComponent<UnitBace>();
            tarTrs = target.transform;
            var redamegeData = reSetDamage?.Invoke(owner, target);
            if (redamegeData != null && !redamegeData.isDamage)
                return;
            var damage = attack;
            var damType = damageType;
            if (reSetDamage != null)
            {
                damage = redamegeData.damage;
                damType = redamegeData.damageType;
            }
            if (attack > 0) target.TakeDamage(damage, owner, damType);
            if (skillPara != null && (damageType == DamageType.attack || damageType == DamageType.magic))
                skillPara.damageEffects?.Invoke(target, trs);

            if (buffAdd != null)
            {
                BuffBase buff = target.CheckBuffToAdd(buffAdd);
                buff.Init(buffType, target, owner, buffTime, value: buffValue);
            }
        }

        if (h_Prefab != null)
        {
            //獲得Colliderの最近点
            Vector3 tarNearestRadiusPos = tar.ClosestPoint(trs.position);
            Transform ts = PoolManager.Instance.GetObj(h_Prefab);
            ts.position = tarNearestRadiusPos;
            ts.LookAt(tarTrs);
        }
    }

    public void Init(UnitBace owner = null, SkillPara skillPara = null, float destroyTime = -1, GameObject h_Prefab = null, GameObject o_Prefab = null, AudioClip clip = null, float waitTime = 0,
        UnitBace target = null, float attack = 0, DamageType damageType = DamageType.no,
        GameObject buffAdd = null, float buffTime = 0, BuffType buffType = BuffType.minus, float buffValue = 0,
        float startSize = 0, float startLifetime = 0,
        DamageRange range = DamageRange.no, float loopTime = 0f, float rangeInnerRadius = 0, float rangeHeight = 0,
        Func<UnitBace, UnitBace, ReSetDamage> reSetDamage = null,
        Color color = default)
    {
        this.owner = owner;
        this.target = target;
        this.skillPara = skillPara;
        this.h_Prefab = h_Prefab;
        this.o_Prefab = o_Prefab;
        this.clip = clip;
        this.waitTime = waitTime;
        obj.layer = 2;
        if (attack != 0)
        {
            float power = 1;
            if (skillPara != null)
                power = skillPara.power;
            this.attack = attack * power;
            this.damageType = damageType;
        }
        if (buffAdd != null)
        {
            this.buffAdd = buffAdd;
            this.buffType = buffType;
            this.buffTime = buffTime;
            this.buffValue = buffValue;
        }
        this.range = range;
        this.rangeInnerRadius = rangeInnerRadius;
        this.rangeHeight = rangeHeight;
        col = GetComponent<Collider>();

        this.reSetDamage = reSetDamage;

        var tempColor = this.color;
        if (color != default)
            this.color = color;
        else
            this.color = defaultColor;
        if (tempColor != this.color && colorMainList != null && colorMainList.Count > 0)
        {
            for (var i = 0; i < colorMainList.Count; i++)
            {
                var ma = colorMainList[i];
                ma.startColor = color;
            }
        }

        //last----------------------------------------------------------------------------------------

        if (this.waitTime != 0)
        {
            CancelInvoke();
            obj.SetActive(false);
            Invoke("WaitToStart", waitTime);
        }
        if (loopTime > 0)
        {
            this.loopTime = loopTime;
            InvokeRepeating(nameof(RefreshCollider), loopTime, loopTime);
        }

        if (destroyTime != -1)
        {
            this.destroyTime = destroyTime;
            this.disAbleTime = destroyTime;

            if (this.waitTime == 0)
                Able();
            else
            {
                obj.SetActive(false);
                Invoke(nameof(WaitToStart), waitTime);
            }
        }
    }

    public void SetAttackDamage(float attack)
    {
        if (attack != 0)
        {
            float power = 1;
            if (skillPara != null)
                power = skillPara.power;
            this.attack = attack * power;
        }
    }

    protected virtual void RefreshCollider()
    {
        executedList.Clear();
        if (col != null)
        {
            col.enabled = false;
            col.enabled = true;
        }
    }
}
public class ReSetDamage
{
    public float damage;
    public bool isDamage;
    public DamageType damageType;
}