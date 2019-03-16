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
using System.Collections.Generic;

public class MainMediator : Mediator
{
    new public const string NAME = "MainMediator";

    public void InitMainMediator(MainUIForm mainUIForm)
    {
        if(mainUIForm)
        {
            mainUIForm.btnStartBattle.onClick.AddListener(Click_StartBattle);
        }
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
        SendNotification(GlobalSetting.Cmd_StartBattle);
    }
}

