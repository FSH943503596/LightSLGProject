/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-25-15:26:18
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class GameFacade : Facade
{
    protected GameFacade() { }

    private ISceneStateController _SceneStateController;

    new public static IFacade Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null)
                    {
                        m_instance = new GameFacade();
                    }
                }
            }

            return m_instance;
        }
    }

    public ISceneStateController sceneStateController { get => _SceneStateController; set => _SceneStateController = value; }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        RegisterProxy(new PlayerVOProxy());
        //RegisterProxy(new MainBaseVOProxy());
        RegisterProxy(new BuildingVOProxy());
        RegisterProxy(new MapVOProxy());
    }

    protected override void InitializeView()
    {
        base.InitializeView();
        RegisterMediator(new LoginMediator());
        RegisterMediator(new MainMediator());
        RegisterMediator(new ConstructionMediator());
        RegisterMediator(new MobilizeTroopsMediator());
        RegisterMediator(new PlayerBattleInfoMediator());
        RegisterMediator(new BattleCameraMediator());
    }

    protected override void InitializeController()
    {
        base.InitializeController();
        RegisterCommand(GlobalSetting.UI_ConstructionUIForm, typeof(ConstructionCommand));
        RegisterCommand(GlobalSetting.UI_LoginUIForm, typeof(LoginUIFormCommand));
        RegisterCommand(GlobalSetting.UI_MainUIForm, typeof(MainUIFormCommand));
        RegisterCommand(GlobalSetting.UI_MobilizeTroopsInfoUIForm, typeof(MobilizeTroopsInfoUIFormCommand));
        RegisterCommand(GlobalSetting.UI_PlayerBattleInfoUIForm, typeof(PlayerBattleInfoUIFormCommand));

        RegisterCommand(GlobalSetting.Cmd_ExitGame, typeof(ExitGameCommand));
        RegisterCommand(GlobalSetting.Cmd_LoginGame, typeof(LoginCommand));
        RegisterCommand(GlobalSetting.Cmd_StartBattle, typeof(StartBattleCommand));
        RegisterCommand(GlobalSetting.Cmd_ShowConstruction, typeof(ShowConstructionCommand));
        RegisterCommand(GlobalSetting.Cmd_CancelConstruction, typeof(CancelConstructionCommand));
        RegisterCommand(GlobalSetting.Cmd_ConfirmConstruction, typeof(ConfirmConstructionCommand));
        RegisterCommand(GlobalSetting.Cmd_PickMainBase, typeof(PickMainBaseCommand));
        RegisterCommand(GlobalSetting.Cmd_MoveTroops, typeof(MoveTroopsCommand));
        RegisterCommand(GlobalSetting.Cmd_MoveCamera, typeof(MoveCameraCommand));
        RegisterCommand(GlobalSetting.Cmd_ReturnLogin, typeof(ReturnLoginCommand));
        RegisterCommand(GlobalSetting.Cmd_UpdateMainBase, typeof(UpdateMainBaseCommand));
        RegisterCommand(GlobalSetting.Cmd_MainBaseChangeOwer, typeof(MainBaseChangeOwerCommand));
    }  
}

