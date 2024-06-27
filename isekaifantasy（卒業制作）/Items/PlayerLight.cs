using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField]
    private float viewRamge = 6;
    private Transform trs;
    private List<UnitBace> enemyHIdeList=new List<UnitBace>();


    private void Start()
    {
        trs = transform;
        var pos = trs.position;
        trs.parent = PlayerManager.Instance.Hero.trs;
        trs.localPosition = pos;
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.enemyList.Count > 0)
        {
            foreach (UnitBace enemy in GameManager.Instance.enemyBaceList)
            {
                float dis = Vector3.Distance(enemy.trs.position, PlayerManager.Instance.Hero.trs.position);
                if (dis < viewRamge)
                {
                    if (enemyHIdeList.Contains(enemy))
                    {
                        enemyHIdeList.Remove(enemy);
                        enemy.OnUnitExitGrass(trs);
                    }
                }
                else
                {
                    if (!enemyHIdeList.Contains(enemy))
                    {
                        enemyHIdeList.Add(enemy);
                        enemy.OnUnitEnterGrass(trs);
                    }
                }
            }
        }
    }
}
