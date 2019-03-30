/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-16:33:01
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using SUIFW;
using System;
using UnityEngine.UI;

public class MainSceneState : ISceneState
{
    public MainSceneState(ISceneStateController controller) : base("Main", controller) { }

    public override void Enter()
    {
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_MainUIForm);
    }

    public override void Exit()
    {
        UIManager.Instance.CloseUIForms(GlobalSetting.UI_MainUIForm);
    }

}

