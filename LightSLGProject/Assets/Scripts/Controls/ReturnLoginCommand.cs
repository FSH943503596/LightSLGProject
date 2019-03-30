/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-20-10:30:28
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class ReturnLoginCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        GameFacade gameFacade = Facade as GameFacade;
        gameFacade.sceneStateController.SetState(new LoginSceneState(gameFacade.sceneStateController), "Login");
    }
}

