using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
/// 
public class ObjectPool {
    
    private GameObject ObjPrefab;
    private List<GameObject> PrefabList=new List<GameObject>();
    private  int MaxPrefabCount = 1000;
    private int ObjectIndex=0;
    public ObjectPool(int maxPrefabCount)
    {
        MaxPrefabCount = maxPrefabCount;
    }
    public bool Contains(GameObject go)
    {
        return PrefabList.Contains(go);
    }
    /// <summary>
    /// 添加物体进入池
    /// </summary>
    /// <param name="obj"></param>
    public void AddObject(GameObject obj)
    {
        if (PrefabList != null && !PrefabList.Contains(obj))
        {
            PrefabList.Add(obj);
            ObjectIndex += 1;
        }
    }
    /// <summary>
    /// 从池中获取物体
    /// </summary>
    /// <param name="ObjName">物体名称</param>
    /// <param name="loadType">加载方式</param>
    /// <param name="objectType">物体类型</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public GameObject GetObject(string ObjName, LoadAssetType loadType,Transform parent)
    {

        GameObject obj = null;
        for (int i = 0; i < PrefabList.Count; i++)
        {
            if (!PrefabList[i])
            {
                PrefabList.RemoveAt(i);
                i--;
                continue;
            }
            if (!PrefabList[i].activeSelf)
            {
                obj = PrefabList[i];
                if (parent) obj.transform.parent = parent;
                obj.SetActive(true);
                break;
            }
        }
        if (obj == null)
        {
            if (PrefabList.Count >= MaxPrefabCount)
            {
                Debug.Log(ObjName + "maxcount" + MaxPrefabCount + "PrefabList.Count" + PrefabList.Count);
                GameObject.Destroy(PrefabList[0]);
                PrefabList.RemoveAt(0);
            }
            if (loadType == LoadAssetType.Normal)
            {
                if (ResourcesMgr.Instance.Load<GameObject>(ObjName) != null)
                {
                    ObjPrefab = ResourcesMgr.Instance.Load<GameObject>(ObjName).res;
                }
              
            }
            else
            {
                  ResourcesMgr.Instance.LoadAsync<GameObject>(ObjName,(res)=>
                {
                    if (res != null)
                    {
                        ObjPrefab = res.res;
                    }
                });
            }
           
            if (ObjPrefab == null)
            {
                Debug.LogError(ObjName + "--->物体未加载成功");
            }
            if (ObjPrefab != null)
            {
                obj = GameObject.Instantiate(ObjPrefab,parent);
                obj.name = ObjName + "_" + ObjectIndex;
                PrefabList.Add(obj);
                
                ObjectIndex += 1;            
            }            
        }  
        return obj;
    }
    /// <summary>
    /// 隐藏物体
    /// </summary>
    /// <param name="go"></param>
    public void HideObject(GameObject go)
    {  
        if (PrefabList.Contains(go))
        {
            go.SetActive(false);
        }
    }
    /// <summary>
    /// 删除物体
    /// </summary>
    /// <param name="go">物体</param>
    public void DestroyObject(GameObject go)
    {
        if (PrefabList.Contains(go))
        {
            PrefabList.Remove(go);
            GameObject.Destroy(go);
        }
    }
    /// <summary>
    /// 隐藏池中所有物体
    /// </summary>
    public void HideAllObject()
    {
        for (int i = 0; i < PrefabList.Count; i++)
        {
            if (PrefabList[i].activeSelf)
            {
                PrefabList[i].SetActive(false);
            }
        }
    }
}
