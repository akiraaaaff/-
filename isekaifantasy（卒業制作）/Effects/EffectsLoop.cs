using System.Collections.Generic;
using UnityEngine;

public class EffectsLoop : EffectsBase
{
    protected float noLoopDestroyTime;
    [SerializeField]
    protected List<ParticleSystem> loopParticleList;
    protected List<ParticleSystem.MainModule> loopMainList;


    protected override void Awake()
    {
        base.Awake();
        noLoopDestroyTime = destroyTime;

        if (loopParticleList != null && loopParticleList.Count > 0)
        {
            loopMainList = new List<ParticleSystem.MainModule>();
            for (var i = 0; i < loopParticleList.Count; i++)
                loopMainList.Add(loopParticleList[i].main);
        }
    }

    private void SetParticlesLoop(bool isLife)
    {
        if (loopMainList != null && loopMainList.Count > 0)
        {
            for (var i = 0; i < loopMainList.Count; i++)
            {
                var ma = loopMainList[i];
                ma.loop = isLife;
            }
        }
    }

    public override void DestroyByPool()
    {
        isAble = false;
        SetParticlesLoop(false);
        CancelInvoke();
        Invoke(nameof(NoloopDestroyByPool), noLoopDestroyTime);
    }
    protected virtual void NoloopDestroyByPool()
    {
        CancelInvoke();
        SetParticlesLoop(true);
        ReSet();
        trs.gameObject.SetActive(false);
        PoolManager.Instance.PushObj(trs);
    }
}
