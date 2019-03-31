using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapUIForm : BaseUIForm
{
    [SerializeField] private Button _BtnOpenMap;
    [SerializeField] private Image _ImgMap;

 

    public Button btnOpenMap { get { return _BtnOpenMap; } }
    public Image imgMap { get { return _ImgMap; } }

}
