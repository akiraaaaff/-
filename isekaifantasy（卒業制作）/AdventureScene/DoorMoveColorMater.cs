using System.Collections.Generic;
using UnityEngine;

public class DoorMoveColorMater : DoorBace
{
    private List<Material> materialList = new List<Material>();
    [SerializeField]
    private List<GameObject> materObjList = new List<GameObject>();
    [SerializeField]
    private Color openColor;
    [SerializeField]
    private Color closeColor;
    private int times;


    private void Start()
    {
        for (int i = 0; i < materObjList.Count; i++)
        {
            Material material = materObjList[i].GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", closeColor);
            materialList.Add(material);
        }
    }

    private void FixedUpdate()
    {
        if (isOpen == true)
        {
            var color = Color.Lerp(closeColor, openColor, times/speed);
            times++;
            if (color!= openColor)
            {
                foreach(var material in materialList)
                    material.SetColor("_BaseColor", color);
            }
            else
            {
                isOpen = false;
                times=0;
            }
        }
        else
        if(isClose == true)
        {
            var color = Color.Lerp(openColor, closeColor, times / speed);
            times++;
            if (color != closeColor)
            {
                foreach (var material in materialList)
                    material.SetColor("_BaseColor", color);
            }
            else
            {
                isClose = false;
                times = 0;
            }
        }
    }
}
