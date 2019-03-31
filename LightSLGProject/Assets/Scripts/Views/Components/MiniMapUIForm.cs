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
 

    public Button btnOpenMap { get { return _BtnOpenMap; } }
    public Image imgMap { get { return _ImgMap; } }

    public Action<GameObject> OnUpdateMainBase;

    public GameObject CreateMainBase()
    {
        var go = Instantiate(_MainBase);
        go.transform.parent = _MainBase.transform.parent;
        go.transform.localScale = Vector3.one;
        go.SetActive(true);

        Debug.Log("´´½¨");

        return go;
    }
   

}
