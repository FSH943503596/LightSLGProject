/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-11:35:50
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class LoginUIFormCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        SendNotification(GlobalSetting.Msg_InitLoginMediator, notification.Body);
    }
}

