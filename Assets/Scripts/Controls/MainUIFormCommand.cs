/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-14:28:04
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class MainUIFormCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        SendNotification(GlobalSetting.Msg_InitMainMediator, notification.Body);
    }
}

