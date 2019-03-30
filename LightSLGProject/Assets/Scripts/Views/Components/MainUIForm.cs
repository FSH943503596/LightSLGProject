/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-14:25:33
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using SUIFW;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIForm : BaseUIForm
{
    [SerializeField] private Button _BtnStartBattle;
    [SerializeField] private Button _BtnReturnLogin;
    [SerializeField] private Toggle _TglSmall;
    [SerializeField] private Toggle _TglNormal;
    [SerializeField] private Toggle _TglBig;
    [SerializeField] private Toggle _TglHuge;

    public Button btnStartBattle => _BtnStartBattle;
    public Button BtnReturnLogin => _BtnReturnLogin;
    public Toggle TglSmall => _TglSmall;
    public Toggle TglNormal => _TglNormal;
    public Toggle TglBig => _TglBig;
    public Toggle TglHuge => _TglHuge;
}

