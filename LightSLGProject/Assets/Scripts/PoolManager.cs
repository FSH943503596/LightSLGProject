using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载场景方式
/// </summary>
public enum LoadSceneType
{
    Normal,
    Async
}
/// <summary>
/// 加载资源方式
/// </summary>
public enum LoadAssetType
{
    Normal,
    Async
}


/// <summary>
/// 对象池管理类
/// </summary>
public class PoolManager
{
    public static readonly PoolManager Instance = new PoolManager();

    private PoolManager() { }

    private Dictionary<string, ObjectPool> poolDict = new Dictionary<string, ObjectPool>();
    /// <summary>
    /// 初始化池
    /// </summary>
    /// <param name="maxPrefabCount">池中预设最大数量</param>
    /// <param name="poolName">池的名称</param>
    public void InitPool(int maxPrefabCount, string poolName)
    {
        if (!poolDict.ContainsKey(poolName))
        {
            poolDict.Add(poolName, new ObjectPool(maxPrefabCount));
        }

    }
    /// <summary>
    /// 从池中获取物体   一个物体对应一个当前物体的池
    /// </summary>
    /// <param name="poolName">物体名/  即池名</param>
    /// <param name="loadType">加载方式</param>
    /// <param name="objectType">生成物体类型</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public GameObject GetObject(string poolName, LoadAssetType loadType, Transform parent)
    {
        if (!poolDict.ContainsKey(poolName))
        {
            poolDict.Add(poolName, new ObjectPool(1000));
        }
        ObjectPool pool = poolDict[poolName];
        return pool.GetObject(poolName, loadType, parent);
    }
    /// <summary>
    /// 从池中获取物体   一个物体对应一个当前物体的池
    /// </summary>
    /// <param name="poolName">物体名/  即池名</param>
    /// <param name="InitPos">初始位置</param>
    /// <param name="loadType">加载方式</param>
    /// <param name="generateType">生成物体类型</param>
    /// <returns></returns>
    public GameObject GetObject(string poolName, Vector3 InitPos, LoadAssetType loadType = LoadAssetType.Normal)
    {
        if (!poolDict.ContainsKey(poolName))
        {
            poolDict.Add(poolName, new ObjectPool(1000));
        }
        ObjectPool pool = poolDict[poolName];
        GameObject obj = pool.GetObject(poolName, loadType, null);
        obj.transform.position = InitPos;
        return obj;
    }

    /// <summary>
    /// 缓存指定数量的对象
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="count"></param>
    public void IncreaseObjectCache(string poolName, int count)
    {
        if (!poolDict.ContainsKey(poolName))
        {
            poolDict.Add(poolName, new ObjectPool(1000 > count ? 1000: count));
        }
        ObjectPool pool = poolDict[poolName];
        GameObject[] gos = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            GameObject obj = pool.GetObject(poolName, LoadAssetType.Normal, null);
            gos[i] = obj;
            obj.transform.position = Vector3.one * -1000;
        }
        for (int i = 0; i < count; i++)
        {
            gos[i].SetActive(false);
        }
    }

    /// <summary>
    /// 添加物体进入池
    /// </summary>
    /// <param name="poolName">池名</param>
    /// <param name="obj">物体</param>
    public void AddObj(string poolName, GameObject obj)
    {
        if (!poolDict.ContainsKey(poolName))
        {
            poolDict.Add(poolName, new ObjectPool(1000));
        }
        ObjectPool pool = poolDict[poolName];
        pool.AddObject(obj);
    }
    /// <summary>
    /// 隐藏物体
    /// </summary>
    /// <param name="go">物体</param>
    public void HideObjet(GameObject go)
    {
        foreach (ObjectPool p in poolDict.Values)
        {
            if (p.Contains(go))
            {
                p.HideObject(go);
                return;
            }
        }
    }
    /// <summary>
    /// 删除物体
    /// </summary>
    /// <param name="go">物体</param>
    public void DestroyObjet(GameObject go)
    {
        foreach (ObjectPool p in poolDict.Values)
        {
            if (p.Contains(go))
            {
                p.DestroyObject(go);
                return;
            }
        }
    }
    /// <summary>
    /// 隐藏池中所有物体
    /// </summary>
    /// <param name="poolName">池名</param>
    public void HideAllObject(string poolName)
    {
        if (!poolDict.ContainsKey(poolName))
        {
            return;
        }
        ObjectPool pool = poolDict[poolName];
        pool.HideAllObject();
    }
    public void HideAllObject()
    {
        foreach (string item in poolDict.Keys)
        {
            ObjectPool pool = poolDict[item];
            pool.HideAllObject();
        }
    }   
    public void ClearData()
    {
        poolDict.Clear();
    }

}
