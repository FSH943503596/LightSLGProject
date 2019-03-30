/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-11:35:11
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class LoginCommand:SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        //TODO 判断用户信息

        //登录成功，跳转长江
        GameFacade gameFacade = Facade as GameFacade;
        gameFacade.sceneStateController.SetState(new MainSceneState(gameFacade.sceneStateController), "Main");
    }
}

