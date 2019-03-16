/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-15-14:08:50
 *	Vertion: 1.0	
 *
 *	Description:
 *      保存由策划规定的游戏参数
*/

using System;
using System.Collections.Generic;

public enum E_Building : byte
{
    None,
    MainBase,
    FarmLand,
    GoldMine,
    MilitaryCamp
}
public static class GlobalSetting
{  
    //场景Tag定义
    public static readonly string TAG_TILE_PARENT_NAME = "_TagTileParent";
    //战斗场景的相机
    public static readonly string TAG_BATTLE_SCENE_CAMERA_NAME = "_TagBattleSceneCamera";
    //战斗场景建筑层Tag
    public static readonly string TAG_BUILDING_PARENT_NAME = "_TagBuildingParent";
    //战斗场景建筑层Tag
    public static readonly string TAG_SOLDIERS_PARENT_NAME = "_TagSoldiersParent";

    //地面碰撞层
    public static readonly string LAYER_MASK_NAME_GROUND = "Ground";

    //主城初始半径大小
    public static readonly int MAINBASE_ORIGINAL_RADIUS = 4;
    //初始主城地块信息，打印地图时，下方为第一行
    public static readonly SByte[,] MAINBASE_ORIGINAL_TILES = new SByte[,] 
    {
        {-1, -1, -1, 2, -1, -1, -1},
        {-1, -1,  2, 2,  2, -1, -1},
        {-1,  2,  2, 2,  2,  2, -1},
        { 2,  2,  2, 2,  2,  2,  2},
        {-1,  2,  2, 2,  2,  2, -1},
        {-1, -1,  2, 2,  2, -1, -1},
        {-1, -1, -1, 2, -1, -1, -1},
    };
    //建筑预制体名称定义
    public static readonly Dictionary<E_Building, string> BuildingNames = new Dictionary<E_Building, string>()
    {
        { E_Building.None , "" },
        { E_Building.MainBase , "MainBase" },
        { E_Building.MilitaryCamp , "MilitaryCamp" },
        { E_Building.FarmLand , "FarmLand" },
        { E_Building.GoldMine , "GoldMine" },
    };

    #region 消息定义

    //MVC Commad
    public const string Cmd_ExitGame = "Cmd_ExitGame";
    public const string Cmd_LoginGame = "Cmd_LoginGame";
    public const string Cmd_StartBattle = "Cmd_StartBattle";
    public const string Cmd_Construction = "Cmd_Construction";
    public const string Cmd_ShowConstruction = "Cmd_ShowConstruction";
    public const string Cmd_CancelConstruction = "Cmd_CancelConstruction";
    public const string Cmd_ConfirmConstruction = "Cmd_ConfirmConstruction";
    public const string Cmd_PickMainBase = "Cmd_PickMainBase";
    public const string Cmd_MoveTroops = "Cmd_MoveTroops";
    public const string Cmd_MoveCamera = "Cmd_MoveCamera";

    //MVC Msg
    public const string Msg_InitConstructionMediator = "Msg_InitConstructionMediator";
    public const string Msg_InitLoginMediator = "Msg_InitLoginMediator";
    public const string Msg_InitMainMediator = "Msg_InitMainMediator";
    public const string Msg_InitMobilizeTroopsMediator = "Msg_InitMobilizeTroopsMediator";
    public const string Msg_BuildBuilding = "Msg_BuildBuilding";
    public const string Msg_PickMainBase = "Msg_PickMainBase";
    #endregion

    #region UIForm定义及初始化Command
    public const string UI_ConstructionUIForm = "ConstructionUIForm";             //建造界面
    public const string UI_LoginUIForm = "LoginUIForm";                           //登录界面
    public const string UI_MainUIForm = "MainUIForm";                             //出兵信息界面
    public const string UI_MobilizeTroopsInfoUIForm = "MobilizeTroopsInfoUIForm"; //主界面
    #endregion
}

