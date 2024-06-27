using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// スクリプトで全ての弾幕に追加し、制御する。
/// </summary>
public class BulletBase : MonoBehaviour
{
    [HideInInspector] public UnitBace owner;
    [HideInInspector] public UnitBace target;
    [HideInInspector] public UnitBace tarChase;
    [HideInInspector] public SkillPara skillPara;
    [HideInInspector] public float attack;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public float speed;
    [HideInInspector] public float destroyDis;
    [HideInInspector]
    public bool collisionDestroy;
    [HideInInspector] public float waitTime;
    [HideInInspector] public int lv;
    [HideInInspector] public BulletType bulletType;
    [HideInInspector] public float damageAttenuation;
    [HideInInspector] public List<UnitBace> groupDamageList;
    [HideInInspector] public GameObject i_Prefab;
    [HideInInspector] public GameObject o_Prefab;
    private bool o_PrefabStay;
    [HideInInspector] public GameObject h_Prefab;

    [HideInInspector] public float dis;
    private List<Collider> executedList;
    private Collider col;
    private bool beDead;

    [HideInInspector] public Transform trs;

    [HideInInspector] public UnityAction<BulletBase> destroyEffects;

    [SerializeField]
    protected List<ParticleSystem> colorParticleList;
    protected List<ParticleSystem.MainModule> colorMainList;
    [HideInInspector] public Color defaultColor;
    [HideInInspector] public Color color = default;

    /// <summary>
    /// 弾幕クリエイトされた時に実行。
    /// </summary>
    /// <param name="owner">発射者</param>
    /// <param name="bulletType">飛行とダメッジのタイプ</param>
    /// <param name="skillPara">このスキルの効果比率</param>
    /// <param name="attack">ダメッジ</param>
    /// <param name="speed">飛行速度</param>
    /// <param name="destroyDis">飛行距離</param>
    /// <param name="destroyActive">プールで削除の時SetActive</param>
    /// <param name="damageType">ダメッジ＝スキルOｒアタック</param>
    /// <param name="target">追跡ターゲット</param>
    /// <param name="damageAttenuation">二回目のダメッジ</param>
    /// <param name="waitTime">発射の遅延</param>
    /// <param name="groupDamageList">同じ弾幕与えたダメッジのターゲット</param>
    /// <param name="damageEffects">ダメッジの特殊効果</param>
    /// <param name="destroyEffects">削除時の特殊効果</param>
    public void Init(
        UnitBace owner,
        UnitBace target,
        BulletType bulletType = BulletType.Solo,
        SkillPara skillPara = null,
        float attack = 10f,
        float speed = 10,
        float destroyDis = 5f,
        bool collisionDestroy = false,
        int lv = 0,
        DamageType damageType = DamageType.attack,
        UnitBace tarChase = null,
        float damageAttenuation = 0,
        float waitTime = 0,
        List<UnitBace> groupDamageList = null,
        GameObject i_Prefab = null,
        GameObject o_Prefab = null,
        bool o_PrefabStay = false,
        GameObject h_Prefab = null,
        UnityAction<BulletBase> destroyEffects = null,
        Color color = default
        )
    {
        trs = transform;
        this.owner = owner;
        this.target = target;
        this.tarChase = tarChase;
        this.skillPara = skillPara;
        this.damageType = damageType;
        float power = 1;
        if (skillPara != null)
            power = skillPara.power;
        this.attack = attack * power;
        this.speed = speed;
        this.destroyDis = destroyDis;
        this.collisionDestroy = collisionDestroy;
        this.waitTime = waitTime;
        this.lv = owner.bulletsLevel;
        this.bulletType = bulletType;
        this.damageAttenuation = damageAttenuation;
        this.groupDamageList = groupDamageList;
        this.i_Prefab = i_Prefab;
        this.o_Prefab = o_Prefab;
        if (o_Prefab != null)
        {
            this.o_PrefabStay = o_PrefabStay;
            if (o_PrefabStay)
            {
                var tarpos = trs.position + (trs.rotation * new Vector3(0f, 0.2f, destroyDis));
                if (target != null)
                    tarpos = target.trs.position;
                trs.LookAt(tarpos);
            }
        }
        this.h_Prefab = h_Prefab;

        col = GetComponent<Collider>();
        dis = 0;
        beDead = false;


        if (owner.friendsGroup == GameManager.Instance.friendsList) GameManager.Instance.friendsBulletsList.Add(col);
        else if (owner.friendsGroup == GameManager.Instance.enemyList)
        {
            GameManager.Instance.enemyBulletsList.Add(col);
            this.speed *= 0.5f;
        }

        this.destroyEffects = destroyEffects;


        if (i_Prefab != null)
        {
            Transform ts = PoolManager.Instance.GetObj(i_Prefab);
            ts.position = trs.position;
            ts.rotation = trs.rotation;
            if (color != default)
            {
                EffectsBase eff = ts.GetComponent<EffectsBase>();
                eff.Init(color: color);
            }
        }

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

        InvokeRepeating(nameof(BulletRun), waitTime, GameManager.Instance.bulletLoopTime);
    }

    private void Awake()
    {
        executedList = new List<Collider>();

        if (colorParticleList != null && colorParticleList.Count > 0)
        {
            colorMainList = new List<ParticleSystem.MainModule>();
            for (var i = 0; i < colorParticleList.Count; i++)
                colorMainList.Add(colorParticleList[i].main);
        }
    }

    /// <summary>
    /// ダメッジの特殊効果。
    /// </summary>
    private void TakeEffects(Collider tar)
    {
        if (executedList.Contains(tar) == false)
        {
            executedList.Add(tar);
            UnitBace unitTar = tar.GetComponent<UnitBace>();
            if (groupDamageList == null)
            {
                unitTar.TakeDamage(attack, owner, damageType);
                skillPara.damageEffects?.Invoke(unitTar, trs);
            }
            else if (groupDamageList.Contains(unitTar) == false)
            {
                groupDamageList.Add(unitTar);
                unitTar.TakeDamage(attack, owner, damageType);
                skillPara.damageEffects?.Invoke(unitTar, trs);
            }
            else if (damageAttenuation > 0)
            {
                unitTar.TakeDamage(attack * damageAttenuation, owner);
                skillPara.damageEffects?.Invoke(unitTar, trs);
            }
            if (h_Prefab != null)
            {
                //獲得Colliderの最近点
                Vector3 tarNearestRadiusPos = tar.ClosestPoint(trs.position);
                Transform ts = PoolManager.Instance.GetObj(h_Prefab);
                ts.position = tarNearestRadiusPos;
                ts.rotation = trs.rotation;
            }
        }
    }

    protected virtual void BulletRun()
    {
        float nowDis = speed * GameManager.Instance.bulletLoopTime;
        dis += nowDis;
        if (tarChase != null) transform.LookAt(tarChase.trs);
        trs.Translate(Vector3.forward * nowDis);

        if (dis >= destroyDis && destroyDis > 0)
        {
            if ((damageType == DamageType.attack ||
                damageType == DamageType.magicArrow) &&
                owner != null &&
                executedList.Count == 0)
            {
                owner.attackAfterShockEffects?.Invoke(trs);
                skillPara.damageEffects?.Invoke(null, trs);
            }
            DestroyByPool();
        }
    }

    private void OnTriggerEnter(Collider tar)
    {
        if (GameManager.Instance.canNotAtkList.Contains(tar)) return;
        if (owner.enemyGroup.Contains(tar))
        {
            if (bulletType == BulletType.Line) TakeEffects(tar);
            else DestroyByPool(tar);
        }
        else if (GameManager.Instance.friendsCanAtkList.Contains(tar) && owner.col != tar && bulletType == BulletType.Line)
        {
            TakeEffects(tar);
        }
        else if ((tar.tag == Tags.obj.ToString() || tar.tag == Tags.canBreakObj.ToString())
            && (GameManager.Instance.enemyBulletsList.Contains(col) || collisionDestroy))
        {
            if ((damageType == DamageType.attack ||
                damageType == DamageType.magicArrow) &&
                owner != null &&
                executedList.Count == 0)
                owner.attackAfterShockEffects?.Invoke(trs);
            skillPara.damageEffects?.Invoke(null, trs);

            if (tar.tag == Tags.canBreakObj.ToString())
                tar.GetComponent<CanBreakItems>().TakeDamage(1, trs);

            DestroyByPool();
        }
    }
    //private void OnTriggerStay(Collider tar)
    //{
    //    if (dis > 1.5f && tar.tag == "obj")
    //        DestroyByPool();
    //}

    /// <summary>
    /// メモリープールで削除。
    /// </summary>
    public void DestroyByPool(Collider tar = null)
    {
        if (beDead == true) return;
        beDead = true;
        CancelInvoke();

        if (o_Prefab != null)
        {
            Transform ts = PoolManager.Instance.GetObj(o_Prefab);
            ts.position = trs.position;
            ts.rotation = trs.rotation;
            if (o_PrefabStay && tar != null) ts.parent = tar.transform;
            if (bulletType == BulletType.Aoe)
            {
                EffectsBase eff = ts.GetComponent<EffectsBase>();
                eff.Init(owner, skillPara: skillPara, attack: attack, damageType: damageType
                    , color: color);
            }
            else 
            if (color != default)
                {
                    EffectsBase eff = ts.GetComponent<EffectsBase>();
                    eff.Init(color: color);
                }
        }
        if (tar != null && tar.tag != Tags.obj.ToString()
            && bulletType == BulletType.Solo) TakeEffects(tar);

        destroyEffects?.Invoke(this);

        executedList.Clear();

        if (GameManager.Instance.friendsBulletsList.Contains(col)) GameManager.Instance.friendsBulletsList.Remove(col);
        if (GameManager.Instance.enemyBulletsList.Contains(col)) GameManager.Instance.enemyBulletsList.Remove(col);
        Invoke(nameof(DestroyWait), 0.1f);
    }
    private void DestroyWait()
    {
        this.skillPara = null;
        this.damageType = 0;
        this.attack = 0;
        this.speed = 0;
        this.destroyDis = 0;
        this.waitTime = 0;
        this.lv = 0;
        this.bulletType = 0;
        this.damageAttenuation = 0;
        this.groupDamageList = null;
        this.i_Prefab = null;
        this.o_Prefab = null;
        this.collisionDestroy = false;
        this.destroyEffects = null;
        gameObject.SetActive(false);
        PoolManager.Instance.PushObj(trs);
    }


    private void OnDestroy()
    {
        executedList.Clear();
        CancelInvoke();

        if (owner.friendsGroup == GameManager.Instance.friendsList) GameManager.Instance.friendsBulletsList.Remove(col);
        else if (owner.friendsGroup == GameManager.Instance.enemyList) GameManager.Instance.enemyBulletsList.Remove(col);
    }
}
