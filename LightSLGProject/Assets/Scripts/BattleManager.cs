/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-14-15:39:49
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using SUIFW;
using UnityEngine;

public class BattleManager : IBattleManager
{
    public static readonly BattleManager Instance = new BattleManager();

    private BattleManager() { }

    private int _MapWidth;
    private int _MapHeight;

    public BattleMapSystem mapSystem;
    public BattleBuildingSystem buildingSystem;
    public BattlePlayerSystem playerSystem;
    public SLGInputSystem inputSystem;
    public BattleTroopSystem troopSystem;
    public bool isBattleOver { get; set; }
    public int mapWidth => _MapWidth;
    public int mapHeight => _MapHeight;

    public void Initinal()
    {
        isBattleOver = false;

        mapSystem = new BattleMapSystem(this);
        mapSystem.Initialize();
        buildingSystem = new BattleBuildingSystem(this);
        buildingSystem.Initialize();
        playerSystem = new BattlePlayerSystem(this);
        playerSystem.Initialize();
        inputSystem = new SLGInputSystem(this);
        inputSystem.Initialize();
        troopSystem = new BattleTroopSystem(this);
        troopSystem.Initialize();

        //显示初始化战斗面板
        ShowBattleUI();
        GameFacade.Instance.SendNotification(GlobalSetting.Msg_StartBattle);
        //初始化所有角色
        playerSystem.InitPlayers();
        GameFacade.Instance.SendNotification(GlobalSetting.Mst_PlayerGenerateComplete);
        //发送初始化战斗场景相机
        GameFacade.Instance.SendNotification(GlobalSetting.Msg_InitBattleCameraMediator);
        //调整相机位置到玩家位置
        //mapSystem.SetCameraPosition(playerSystem.UserMainBasePosition);
        FocuseUserMainBase();
        //生成地图
        mapSystem.PrintMap();
    }

    private void FocuseUserMainBase()
    {
        var position = TwoMsgParamsPool<float, float>.Instance.Pop();
        position.InitParams(playerSystem.UserMainBasePosition.x, playerSystem.UserMainBasePosition.z);
        GameFacade.Instance.SendNotification(GlobalSetting.Msg_CameraFocusPoint, position);
    }

    private void ShowBattleUI()
    {
        //显示建造面板
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_ConstructionUIForm);
        //显示玩家战斗信息面板
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_PlayerBattleInfoUIForm);

        //初始化面板
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_MobilizeTroopsInfoUIForm);
        UIManager.Instance.CloseUIForms(GlobalSetting.UI_MobilizeTroopsInfoUIForm);
    }

    public void Release()
    {
        GameFacade.Instance.SendNotification(GlobalSetting.Msg_EndBattle);

        inputSystem.Release();
        troopSystem.Release();
        playerSystem.Release();
        buildingSystem.Release();
        mapSystem.Release();
       
        PoolManager.Instance.ClearData();     
        System.GC.Collect();
    }

    public void Update()
    {
        if (isBattleOver) return;

        inputSystem.Update();
        mapSystem.Update();
        troopSystem.Update();
        buildingSystem.Update();
        playerSystem.Update();
    }

    public void ShowConstraction(Transform builidTF)
    {
        inputSystem.longTapItem = builidTF;
    }

    public void CancelConstraction()
    {
        inputSystem.longTapItem = null;
        inputSystem.dragItem = null;
    }

    public void SetMapRect(int width, int height)
    {
        _MapWidth = width;
        _MapHeight = height;
    }

    public void SetBattleResult(bool isUserWin)
    {
        if (isUserWin)
        {
            UIManager.Instance.ShowUIForms(GlobalSetting.UI_VictoryUIForm);
        }
        else
        {
            UIManager.Instance.ShowUIForms(GlobalSetting.UI_FailedUIForm);
        }

        isBattleOver = true;
    }
}