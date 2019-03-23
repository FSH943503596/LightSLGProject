/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-14:34:09
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class StartBattleCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        OneMsgParams<ushort> mapsize = notification.Body as OneMsgParams<ushort>;
        if (mapsize != null)
        {
            GameFacade gameFacade = Facade as GameFacade;
            //各种战斗判断
            BattleManager.Instance.SetMapRect(mapsize.parameter, mapsize.parameter);
            gameFacade.sceneStateController.SetStateAsync(new BattleSceneState(gameFacade.sceneStateController, BattleManager.Instance), "Battle");
        } 
    }
}

