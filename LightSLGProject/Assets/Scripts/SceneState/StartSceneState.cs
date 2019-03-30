/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-16:32:03
 *	Vertion: 1.0	
 *
 *	Description:
 *      游戏Logo，开始视频，广告等播放
 *      游戏基本数据的加载，初始化
*/

using PureMVC.Interfaces;
using StaticData;
using SUIFW;
using System;

public class StartSceneState : ISceneState
{
    public StartSceneState(ISceneStateController controller) : base("Start", controller) { }

    public override void Enter()
    {
        //加载资源配置
        ResourcesMgr.Instance.LoadConfigs();
        //加载配置信息
        StaticDataMgr.mInstance.LoadData();
        //启动MVC框架
        GameFacade gameFacade = GameFacade.Instance as GameFacade;
        gameFacade.sceneStateController = StateController;
        //启动UI框架
        UIManager.Instance.Loadinitialization = gameFacade.SendNotification;
    }

    public override void Update()
    {
        StateController.SetState(new LoginSceneState(StateController), "Login");
    }
}

