/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-20-11:52:15
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class PlayerBattleInfoUIFormCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        PlayerBattleInfoUIForm playerBattleInfoUIForm = notification.Body as PlayerBattleInfoUIForm;
        if (playerBattleInfoUIForm)
        {
            SendNotification(GlobalSetting.Msg_InitPlayerBattleInfoMediator, playerBattleInfoUIForm);
        }
    }
}

