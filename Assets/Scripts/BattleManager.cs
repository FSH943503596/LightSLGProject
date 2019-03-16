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

    public BattleMapSystem mapSystem;
    public BattleBuildingSystem buildingSystem;
    public BattlePlayerSystem playerSystem;
    public SLGInputSystem inputSystem;
    public BattleTroopSystem troopSystem;
    private BattleManager() { }

    public bool IsBattleOver { get; set; }

    public void Initinal()
    {
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
        //初始化所有角色
        playerSystem.InitPlayers();
        //调整相机位置到玩家位置
        mapSystem.SetCameraPosition(playerSystem.UserMainBasePosition);
        //生成地图
        mapSystem.PrintMap();       
    }

    private void ShowBattleUI()
    {
        //显示建造面板
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_ConstructionUIForm);

        //初始化面板
        UIManager.Instance.ShowUIForms(GlobalSetting.UI_MobilizeTroopsInfoUIForm);
        UIManager.Instance.CloseUIForms(GlobalSetting.UI_MobilizeTroopsInfoUIForm);
    }

    public void Release()
    {
        troopSystem.Release();
        mapSystem.Release();      
    }

    public void Update()
    {
        inputSystem.Update();
        mapSystem.Update();
        troopSystem.Update();
    }

    public void ShowConstraction(Transform builidTF)
    {
        inputSystem.LongTapItem = builidTF;
    }

    public void CancelConstraction()
    {
        inputSystem.LongTapItem = null;
        inputSystem.DragItem = null;
    }
}