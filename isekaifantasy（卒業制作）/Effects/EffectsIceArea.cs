using System.Collections.Generic;
using UnityEngine;

public class EffectsIceArea : EffectsLoop
{
    [SerializeField]
    private GameObject objIce;
    private float time;
    private List<EffectsIceArea> effList;

    protected override void Awake()
    {
        base.Awake();
        objIce.SetActive(false);
    }
    protected override void On()
    {
        objIce.SetActive(true);
        base.On();
        time = 0;
        InvokeRepeating(nameof(loop), 0f, GameManager.Instance.invokeLoopTime / 5);
    }
    public void SetList(List<EffectsIceArea> effList)
    {
        this.effList = effList;
    }

    private void OnDestroy()
    {
        if (effList == null)
            return;
        if (effList.Contains(this))
            effList.Remove(this);
    }

    public override void DestroyByPool()
    {
        base.DestroyByPool();
        effList.Remove(this);
    }
    protected override void NoloopDestroyByPool()
    {
        base.NoloopDestroyByPool();
        objIce.SetActive(false);
    }
    private void loop()
    {
        var v = time / 5;
        if (v >= 1)
            return;
        time += GameManager.Instance.invokeLoopTime / 5;
        trs.localScale = new Vector3(v, v, v);
    }
}
