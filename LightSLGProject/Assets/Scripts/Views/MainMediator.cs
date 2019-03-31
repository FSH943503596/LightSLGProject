/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-14:39:04
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class MainMediator : Mediator
{
    new public const string NAME = "MainMediator";

    private ushort _MapSize;

    public void InitMainMediator(MainUIForm mainUIForm)
    {
        if(mainUIForm)
        {
            _MapSize = GlobalSetting.MAP_SMALL_LENGTH;
            mainUIForm.btnStartBattle.onClick.AddListener(Click_StartBattle);
            mainUIForm.BtnReturnLogin.onClick.AddListener(Click_ReturnLogin);
            mainUIForm.TglSmall.onValueChanged.AddListener(p => { if (p) SelectMapSize(GlobalSetting.MAP_SMALL_LENGTH); });
            mainUIForm.TglNormal.onValueChanged.AddListener(p => { if (p) SelectMapSize(GlobalSetting.MAP_NORMAL_LENGTH); });
            mainUIForm.TglBig.onValueChanged.AddListener(p => { if (p) SelectMapSize(GlobalSetting.MAP_BIG_LENGTH); });
            mainUIForm.TglHuge.onValueChanged.AddListener(p => { if (p) SelectMapSize(GlobalSetting.MAP_HUGE_LENGTH); });


        }
    }

    private void SelectMapSize(ushort size)
    {
        _MapSize = size;
    }

    private void Click_ReturnLogin()
    {
        SendNotification(GlobalSetting.Cmd_ReturnLogin);
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitMainMediator
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitMainMediator:
                MainUIForm mainUIForm = notification.Body as MainUIForm;
                if(mainUIForm)
                {
                    InitMainMediator(mainUIForm);
                }
                break;
            default:
                break;
        }
    }
    private void Click_StartBattle()
    {
        var mapsize = OneMsgParamsPool<ushort>.Instance.Pop();
        mapsize.InitParams(_MapSize); 
        SendNotification(GlobalSetting.Cmd_StartBattle, mapsize);
    }
}

