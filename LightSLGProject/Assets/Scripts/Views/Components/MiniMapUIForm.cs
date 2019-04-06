using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapUIForm : BaseUIForm
{
    [SerializeField] private Button _BtnOpenMap;
    [SerializeField] private Image _ImgMap;
    [SerializeField] private GameObject _MainBase;
    [SerializeField] private GameObject _Soldier;


    public Button btnOpenMap { get { return _BtnOpenMap; } }
    public Image imgMap { get { return _ImgMap; } }

    public Action<GameObject> OnUpdateMainBase;

    private List<GameObject> _SoldierList = new List<GameObject>();

    public GameObject CreateMainBase()
    {
        var go = Instantiate(_MainBase);
        go.transform.parent = _MainBase.transform.parent;
        go.transform.localScale = Vector3.one;
        go.SetActive(true);

        Debug.Log("创建主城");

        return go;
    }
    public List<GameObject> CreateTroops(MapSoldierInfo mapSoldierInfo)
    {
        int index = 0;
        _SoldierList.ForEach(p => p.SetActive(false));
        while (mapSoldierInfo.nextNode != null)
        {
            if (index++ < _SoldierList.Count)
            {
                _SoldierList[index].SetActive(true);
                _SoldierList[index].transform.position = mapSoldierInfo.position;
                _SoldierList[index].GetComponent<Image>().color = GlobalSetting.PLAYER_COLOR_LIST[mapSoldierInfo.coloerIndex];
            }
            else
            {
                var go = Instantiate(_Soldier);
                go.SetActive(true);
                go.transform.parent = _Soldier.transform.parent;
                go.transform.localScale = Vector3.one;
                go.transform.position = mapSoldierInfo.position;
                go.GetComponent<Image>().color = GlobalSetting.PLAYER_COLOR_LIST[mapSoldierInfo.coloerIndex];
                _SoldierList.Add(go);
            }
        }
        Debug.Log("创建兵");

        return _SoldierList;



        //foreach (var item in keyValuePairs)
        //{
        //    for (int i = 0; i < item.Value.Count; i++)
        //    {
        //        if (i < _SoldierList.Count)
        //        {
        //            _SoldierList[i].SetActive(true);
        //            _SoldierList[i].transform.position = item.Value[i];
        //            _SoldierList[i].GetComponent<Image>().color = GlobalSetting.PLAYER_COLOR_LIST[item.Key.colorIndex];
        //        }
        //        else
        //        {
        //            var go = Instantiate(_Soldier);
        //            go.SetActive(true);
        //            go.transform.parent = _Soldier.transform.parent;
        //            go.transform.localScale = Vector3.one;
        //            go.transform.position = item.Value[i];
        //            go.GetComponent<Image>().color = GlobalSetting.PLAYER_COLOR_LIST[item.Key.colorIndex];
        //            _SoldierList.Add(go);
        //        }
        //    }
        //}



    }

}
