/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-10:47:12
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class LoginMediator : Mediator
{
    new public static string NAME = "LoginMediator";

    public LoginMediator() : base(NAME) { }

    public void InitLoginMediator(LoginUIForm uIForm) {
        uIForm.btnLogin.onClick.AddListener(OnClick_Login);
        uIForm.btnExit.onClick.AddListener(OnClick_Exit);
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitLoginMediator
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitLoginMediator:
                LoginUIForm loginUIForm = notification.Body as LoginUIForm;
                if (loginUIForm)
                {
                    InitLoginMediator(loginUIForm);
                }
                break;
            default:
                break;
        }
    }

    private void OnClick_Exit()
    {
        SendNotification(GlobalSetting.Cmd_ExitGame);
    }

    private void OnClick_Login()
    {
        SendNotification(GlobalSetting.Cmd_LoginGame);
    }
}

