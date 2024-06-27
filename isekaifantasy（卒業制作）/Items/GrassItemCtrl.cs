using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassItemCtrl : MonoBehaviour {
    
    private int unitsCountInGrass;
    private List<Material> _materialList =new List<Material>();
    private List<Transform> _childList = new List<Transform>();
    private Transform grassGroup;
    private bool isDes = false;
    [SerializeField] RotateType rotate = RotateType.Random;
    private enum RotateType
    {
        None,
        Random,
        Rigtht
    };

    private void Awake()
    {
        grassGroup = transform.parent;
        switch (rotate)
        {
            case RotateType.Random:
                foreach (Transform child in transform)
                {
                    child.rotation *= Quaternion.Euler(0f, Random.Range(-180, 180), 0f);
                    this._materialList.Add(child.GetComponent<Renderer>().sharedMaterial);
                    this._childList.Add(child);
                }
                break;
            case RotateType.Rigtht:
                foreach (Transform child in transform)
                {
                    var angle = Random.Range(0, 4);
                    switch (angle) {
                        case 0:
                            break;
                        case 1:
                            child.rotation *= Quaternion.Euler(0f, 90, 0f);
                            break;
                        case 2:
                            child.rotation *= Quaternion.Euler(0f, 180, 0f);
                            break;
                        case 3:
                            child.rotation *= Quaternion.Euler(0f, 270, 0f);
                            break;

                    }
                    this._materialList.Add(child.GetComponent<Renderer>().sharedMaterial);
                    this._childList.Add(child);
                }
                break;
            case RotateType.None:
                foreach (Transform child in transform)
                {
                    this._materialList.Add(child.GetComponent<Renderer>().sharedMaterial);
                    this._childList.Add(child);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)   //Layer 10 - GrassDetector
            return;

        UnitBace unit = other.GetComponentInParent<UnitBace>();
        if(unit != null)
        {
            unitsCountInGrass++;
            this._Refresh();
            unit.OnUnitEnterGrass(grassGroup,this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 8)
            return;

        UnitBace unit = other.GetComponentInParent<UnitBace>();
        if (unit != null)
        {
            unitsCountInGrass--;
            this._Refresh();
            unit.OnUnitExitGrass(grassGroup, this);
        }
    }
    public void ExitCollider()
    {
        unitsCountInGrass--;
        this._Refresh();
    }

    private void _Refresh()
    {
        if (isDes) return;
        if (this.unitsCountInGrass > 0)
        {
            //that means there is one or more units on grass
            this.FadeTo(0.25f, 0.5f);
        }
        else
        {
            //that means there is no units inside grass
            this.FadeTo(1, 0.5f);
        }
    }


    private IEnumerator _coroutine;
    public void FadeTo(float targetOpacity, float duration)
    {
        if (_coroutine != null)
            this.StopCoroutine(this._coroutine);

        this._coroutine = this._FadeTo(targetOpacity, duration);
        this.StartCoroutine(this._coroutine);
    }

    IEnumerator _FadeTo(float targetOpacity, float duration)
    {
        Color color = _materialList[0].GetColor("_BaseColor");
        float startOpacity = color.a;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration);
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            for (int i = 0; i < _childList.Count; i++)
                _childList[i].GetComponent<Renderer>().material.color = color;

            yield return null;
        }

        if (targetOpacity == 1)
        {
            for (int i = 0; i < _childList.Count; i++)
                _childList[i].GetComponent<Renderer>().material = this._materialList[i];
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        _coroutine = null;
        isDes = true;
    }
}
