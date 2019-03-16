/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-05-10:46:44
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class CancelConstructionCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        BattleManager.Instance.CancelConstraction();
    }
}

