/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-14-16:07:28
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine;

[Obsolete("不再使用该方法")]
public class MoveCameraCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        var position = notification.Body as OneMsgParams<Vector3>;
        if (position != null)
        {
            BattleManager.Instance.mapSystem.MoveCameraToPosition(position.parameter);
        }
    }
}

