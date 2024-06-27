using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private static PoolManager instance;

    private Transform poolObj;

    //カメラ視線外
    private Vector3 defultPos = new Vector3(0, -50, 0);

    //keyはprefabName
    //valueはObject
    private Dictionary<string, List<Transform>> poolDataDic = new Dictionary<string, List<Transform>>();

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PoolManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// ゲットオブジェクト
    /// </summary>
    public Transform GetObj(GameObject prefab)
    {
        Transform trs = null;
        //リストのコンテンツのありかを確かめる
        if (poolDataDic.Count > 0 && poolObj == null)
        {
            //メモリー辞書を削除
            Clear();
        }
        //もしメモリー辞書にこのプレハブあり
        //そして、オブジェクトもあり
        if (poolDataDic.ContainsKey(prefab.name) && poolDataDic[prefab.name].Count > 0)
        {
            //リストの一番めを出して
            trs = poolDataDic[prefab.name][0];
            //リストから一番めを削除
            poolDataDic[prefab.name].RemoveAt(0);
            //親をなしに
            trs.SetParent(null);
        }
        //もしなかった
        else
        {
            //クリエイトして出して
            trs = Object.Instantiate(prefab).transform;
            trs.name = prefab.name;
        }
        //もらったオブジェクトを表示
        trs.gameObject.SetActive(true);
        return trs;
    }

    /// <summary>
    /// オブジェクトをメモリーに入れる
    /// </summary>
    /// <param name="trs"></param>
    public void PushObj(Transform trs)
    {
        //リストのコンテンツのありかを確かめる
        if (poolObj == null)
        {
            poolObj = new GameObject("poolObj").transform;
            poolObj.position = defultPos;
            //もしメモリー辞書あり
            if (poolDataDic.Count > 0)
            {
                //メモリー辞書を削除
                Clear();
            }
        }

        //辞書中のプレハブのありかを確かめる
        if (poolDataDic.ContainsKey(trs.name))
        {
            //中に入れる
            poolDataDic[trs.name].Add(trs);
        }
        //辞書にない場合は
        else
        {
            //クリエイトこのプレハブのリスト
            poolDataDic.Add(trs.name, new List<Transform>() { trs });
        }
        //もしコンテンツの中にプレハブ同じ名前の子供がない場合
        if (poolObj.Find(trs.name) == false)
        {
            Transform child = new GameObject(trs.name).transform;
            child.position = defultPos;
            child.SetParent(poolObj);
        }
        //親を設定
        trs.SetParent(poolObj.Find(trs.name));
        //非表示
        trs.position = defultPos;
    }

    /// <summary>
    /// 全てのデータを削除
    /// </summary>
    public void Clear()
    {
        poolDataDic.Clear();
    }
}
