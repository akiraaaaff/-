using System.Collections.Generic;
using UnityEngine;

public class ItemGroup : MonoBehaviour
{
    [HideInInspector]
    public List<Collider> executedList = new List<Collider>();
    public int damage = 10;
}
