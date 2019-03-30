/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-10:16:49
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class MoveTroopsCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        var threeCmdParams = notification.Body as ThreeMsgParams<MainBaseVO, MainBaseVO, int>;
        if (threeCmdParams != null)
        {
            BattleManager.Instance.troopSystem.CreateTroop(threeCmdParams.First, threeCmdParams.Second, threeCmdParams.Third);
        }
    }
}

