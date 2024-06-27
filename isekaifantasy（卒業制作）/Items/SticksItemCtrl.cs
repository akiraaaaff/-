using System.Collections;
using UnityEngine;

public class SticksItemCtrl : MonoBehaviour
{
    private ItemGroup itemGroup;

    private void Awake()
    {
        itemGroup = transform.parent.GetComponent<ItemGroup>();
    }
    private void OnTriggerStay(Collider tar)
    {
        if (!itemGroup.executedList.Contains(tar) && 
            (GameManager.Instance.enemyList.Contains(tar)|| 
            GameManager.Instance.friendsList.Contains(tar)))
            TakeDamage(tar);
    }

    private void TakeDamage(Collider tar)
    {
        itemGroup.executedList.Add(tar);
        UnitBace target = tar.GetComponent<UnitBace>();
        target.TakeDamage(itemGroup.damage);
        StartCoroutine(ReDamageList(tar));
    }

    private IEnumerator ReDamageList(Collider tar)
    {
        yield return new WaitForSeconds(0.5f);
        itemGroup.executedList.Remove(tar);
    }
}
