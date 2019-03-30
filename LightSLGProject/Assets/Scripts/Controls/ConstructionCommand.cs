/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-27-16:37:50
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class ConstructionCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        SendNotification(GlobalSetting.Msg_InitConstructionMediator, notification.Body);
    }
}

