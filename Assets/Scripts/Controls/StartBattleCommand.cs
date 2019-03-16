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
        GameFacade gameFacade = Facade as GameFacade;
        //各种战斗判断

        gameFacade.sceneStateController.SetStateAsync(new BattleSceneState(gameFacade.sceneStateController, BattleManager.Instance), "Battle");
    }
}

