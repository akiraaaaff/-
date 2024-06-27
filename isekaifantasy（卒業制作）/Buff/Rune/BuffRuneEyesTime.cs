using UnityEngine;
using UnityEngine.UI;

public class BuffRuneEyesTime : BuffBase
{
    [SerializeField]
    private Image circle;
    protected override void BuffLoop()
    {
        base.BuffLoop();
        circle.fillAmount = 1f-timeCount / destroyTime;
    }
}
