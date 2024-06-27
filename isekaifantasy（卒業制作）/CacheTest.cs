using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System;
public class CacheTest : MonoBehaviour
{
    const int ITERATIONS = 1000000;
    // Use this for initialization
    Transform cached;
    [SerializeField]
    Transform trans1;
    [SerializeField]
    Transform trans2;
    [SerializeField]
    GameObject trans3;

    IEnumerator Start()
    {
        cached = transform;
        UnityEngine.Debug.Log("test.........");
        while (true)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.T)) break;
            var sw1 = Stopwatch.StartNew();
            {
                for (int i = 0; i < ITERATIONS; i++)
                    trans1.gameObject.SetActive(true);
            }
            sw1.Stop();
            var sw2 = Stopwatch.StartNew();
            {
                for (int i = 0; i < ITERATIONS; i++)
                    if (!trans2.gameObject.activeSelf) trans2.gameObject.SetActive(true);
            }
            sw2.Stop();
            var sw3 = Stopwatch.StartNew();
            {
                for (int i = 0; i < ITERATIONS; i++)
                    trans3.SetActive(true);
            }
            sw3.Stop();
            var sw4 = Stopwatch.StartNew();
            {
                Transform trans4;
                for (int i = 0; i < ITERATIONS; i++)
                    trans4 = this.transform;
            }
            sw4.Stop();
            //UnityEngine.Debug.Log(ITERATIONS + " iterations");
            UnityEngine.Debug.Log("1" + sw1.ElapsedMilliseconds + "ms");
            UnityEngine.Debug.Log("2" + sw2.ElapsedMilliseconds + "ms");
            UnityEngine.Debug.Log("3" + sw3.ElapsedMilliseconds + "ms");
            //UnityEngine.Debug.Log("4" + sw4.ElapsedMilliseconds + "ms");
        }
    }
}
