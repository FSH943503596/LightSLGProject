/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-16:32:28
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneState : ISceneState
{
    public LoginSceneState(ISceneStateController controller) : base("Login", controller) { }

    public override void Enter()
    {
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_LoginUIForm);
    }
    public override void Exit()
    {
        UIManager.Instance.CloseUIForms(GlobalSetting.UI_LoginUIForm);
    }
}

