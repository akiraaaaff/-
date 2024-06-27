using System.Collections;
using UnityEngine;

public class BuffRuneDoubleMagic : BuffBase
{
    [SerializeField]
    private Transform circleTrs;
    [SerializeField]
    private GameObject circle;
    [SerializeField]
    private GameObject circle1;
    [SerializeField]
    private GameObject circle2;
    [SerializeField]
    private GameObject circle3;
    private int chargeTimes;


    protected override void BuffFirstAdd()
    {
        base.BuffFirstAdd();
        my.useSkillEffects += OnSkill;
        my.useAttackEffects += Plus;
        SetCircle(false);
    }

    protected override void BuffRemove()
    {
        base.BuffRemove();
        my.useSkillEffects -= OnSkill;
        my.useAttackEffects -= Plus;
        SetCircle(false);
    }
    private void OnSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        if (chargeTimes >= 4)
        {
            if (!my.doubleSkillOnly)
                my.doubleSkillOnly = true;
            else
                return;
            var tempPara = new SkillPara();
            tempPara.CopyData(skillPara);
            tempPara.isDouble = true;
            StartCoroutine(DoubleSkill(target, skill, tempPara));
            SetCircle(false);
        }
        else
            Plus(target, skill, skillPara);
    }
    private void Plus(UnitBace target, Skill skill, SkillPara skillPara)
    {
        for (int i = 0; i < (int)value; i++)
            SetCircle(true);
    }

    private IEnumerator DoubleSkill(UnitBace target, Skill skill, SkillPara skillPara)
    {
        yield return new WaitForSeconds(0f);
        my.doubleSkillOnly = false;
        yield return new WaitForSeconds(GameManager.Instance.DoubleSkillWaitTime);
        skill.skill?.Invoke(my, target, skill, skillPara);
    }

    private void SetCircle(bool isOn)
    {
        if (isOn)
        {
            if (chargeTimes >= 4)
                return;
            chargeTimes++;
            if (!circle.activeSelf)
                circle.SetActive(isOn);
            else
            if (!circle1.activeSelf)
                circle1.SetActive(isOn);
            else
            if (!circle2.activeSelf)
                circle2.SetActive(isOn);
            else
            if (!circle3.activeSelf)
                circle3.SetActive(isOn);
        }
        else
        {
            chargeTimes = 0;
            circle.SetActive(isOn);
            circle1.SetActive(isOn);
            circle2.SetActive(isOn);
            circle3.SetActive(isOn);
        }
    }
}
