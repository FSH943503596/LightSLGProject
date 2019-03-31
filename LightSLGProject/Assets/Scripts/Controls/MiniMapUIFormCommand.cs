using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUIFormCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        Debug.Log("Command÷¥––");
        base.Execute(notification);

        SendNotification(GlobalSetting.Msg_InitMiniMapMediator, notification.Body);
    }
}
