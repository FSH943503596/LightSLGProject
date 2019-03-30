using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapMediator : Mediator
{
    new public static string NAME = "MiniMapMediator";

    public MiniMapMediator() : base(NAME) { }

    public void InitMiniMapMediator(MiniMapUIForm uIForm)
    {
        uIForm.btnOpenMap.onClick.AddListener(OnClick_OpenMap);

        //uIForm.mapData = MapVOProxy
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitMiniMapMediator,
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitMiniMapMediator:
                MiniMapUIForm miniMapUIForm = notification.Body as MiniMapUIForm;
                if (miniMapUIForm)
                {
                    InitMiniMapMediator(miniMapUIForm);
                }

                break;
            default:
                break;
        }
    }


    private void OnClick_OpenMap()
    {

    }
}
