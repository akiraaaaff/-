using UnityEngine;

/// <summary>
/// 全てのBUffPrefabに追加し、持っているbuffを制御する。
/// </summary>
public class BuffBase : MonoBehaviour
{
    private BuffType buffType;
    protected UnitBace my;
    [HideInInspector] public UnitBace owner;
    private Transform parent;
    protected float destroyTime;
    protected float loopTime;
    [HideInInspector] public BuffState buffState;
    [HideInInspector] public int buffLv;
    [HideInInspector] public int buffLvMax;
    protected float value;
    protected Skill skill;
    protected SkillPara skillPara;

    protected Transform trs;
    protected float timeCount;

    private void Awake()
    {
        trs = transform;
    }

    public void Init(BuffType buffType, UnitBace my, UnitBace owner, 
        float destroyTime, BuffState buffState = BuffState.start,
        int buffLv=-1,int buffLvMax = 1, float loopTime = 0.1f, float value = 0, 
        Transform parent = null, Skill skill = null,SkillPara skillPara=null)
    {
        this.buffType = buffType;
        this.my = my;
        this.owner = owner;
        if (this.buffLv == 0 || destroyTime > this.destroyTime) this.destroyTime = destroyTime;
        this.buffState = buffState;
        this.buffLvMax = buffLvMax;
        var tempskill = this.skill;
        this.skill = skill;
        if (tempskill != skill)
            BuffAddSkill();
        this.skillPara = skillPara;
        timeCount = 0;

        if (parent == null) parent = my.trs;
        if (parent == my.trs) trs.position = parent.position + Vector3.up * 0.01f;
        else trs.position = parent.position;
        trs.parent = parent;
        this.parent = parent;
        trs.localEulerAngles = Vector3.zero;
        trs.localScale = Vector3.one;
        if (my.materObjList[0].layer!=0)
        {
            gameObject.layer = my.materObjList[0].layer;
            foreach (Transform trans in trs)
            {
                trans.gameObject.layer = my.materObjList[0].layer;
            }
        }

        if (buffLv == -1)
        {
            if (this.buffLv < buffLvMax)
            {
                this.value = value;
                if (this.buffLv == 0)
                    BuffFirstAdd();
                this.buffLv++;
                BuffAdd();
            }
            else if (Mathf.Abs(value) > Mathf.Abs(this.value))
            {
                this.value = value - this.value;
                BuffAdd();
            }
        }
        else
        {
            this.value = value;
            this.buffLv = buffLv;
            BuffAdd();
        }
        if(this.loopTime != loopTime)
        {
            this.loopTime = loopTime;
            CancelInvoke(nameof(BuffRunLoop));
            if (loopTime > 0) InvokeRepeating(nameof(BuffRunLoop), loopTime, loopTime);
        }
    }

    public void ReSet(int buffLv = -1)
    {
        if (buffLv == -1)
        {
            if (this.buffLv < buffLvMax)
                this.buffLv++;
        }else
            this.buffLv = buffLv;

        BuffRset();
    }

    public virtual void BuffRun(UnitBace target = null) { }
    
    /// <summary>
    /// メモリープールで削除。
    /// </summary>
    public void DestroyByPool()
    {
        ExitByPool();
        if (my != null&& my.CheckBuff(name))
            my.BuffDic.Remove(name);
    }

    /// <summary>
    /// メモリープールで削除。
    /// </summary>
    public void ExitByPool()
    {
        buffLv = 0;
        if (gameObject.layer != 0)
        {
            gameObject.layer = 0;
            foreach (Transform trans in trs)
            {
                trans.gameObject.layer = 0;
            }
        }
        BuffRemove();
        CancelInvoke();
        loopTime = 0;
        trs.gameObject.SetActive(false);
        PoolManager.Instance.PushObj(trs);
    }

    protected virtual void BuffLoop() { }

    protected virtual void BuffAdd() { }

    protected virtual void BuffFirstAdd() { }

    protected virtual void BuffAddSkill() { }

    protected virtual void BuffRset() { }

    protected virtual void BuffRemove() { }
    public virtual void BuffSkillRemove(Skill skill) { }

    private void BuffRunLoop()
    {
        timeCount += loopTime;
        BuffLoop();
        if (timeCount >= destroyTime && destroyTime != 0)
        {
            DestroyByPool();
        }
    }

    protected float SetMoveSpeedByMin(float speed)
    {
        if (my.MoveSpeed + speed < GameManager.Instance.minAttackSpeed)
        {
            speed = GameManager.Instance.minMoveSpeed - my.MoveSpeed;
            if (speed == 0) return speed;
        }
        my.MoveSpeed += speed;
        return speed;
    }
    protected float SetAttackSpeedByMin(float attackSpeed)
    {
        if (my.AttackSpeed + attackSpeed < GameManager.Instance.minAttackSpeed)
        {
            attackSpeed = GameManager.Instance.minAttackSpeed - my.AttackSpeed;
            if (attackSpeed == 0) return attackSpeed;
        }
        my.AttackSpeed += attackSpeed;
        return attackSpeed;
    }
}
