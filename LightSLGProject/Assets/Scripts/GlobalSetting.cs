/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-15-14:08:50
 *	Vertion: 1.0	
 *
 *	Description:
 *      保存由策划规定的游戏参数
*/

using StaticData;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    #region BattleCamera
    /// <summary>
    /// 相机视野水平面，Z轴最小偏移值
    /// </summary>
    public static readonly float CAMERA_MIN_Z_OFFSET = 5;
    /// <summary>
    /// 相机事业水平面，Z轴最大偏移值
    /// </summary>
    public static readonly float CAMERA_MAX_Z_OFFSET = 15;
    /// <summary>
    /// 相机最小可视角度
    /// </summary>
    public static readonly float CAMERA_MIN_ANGLE = 30;
    /// <summary>
    /// 相机最大可视角度
    /// </summary>
    public static readonly float CAMERA_MAX_ANGLE = 60;
    /// <summary>
    /// 相机的固定高度
    /// </summary>
    public static readonly float CAMERA_HIGHT = 10;
    #endregion

    #region Tile
    /// <summary>
    /// 山地地形对产出影响（粮食Val|粮食Per|金矿Val|金矿Per）
    /// </summary>
    public static readonly short[] TILE_MOUNTAIN_OUTPUT_EFFECT;
    /// <summary>
    /// 丘陵地形对产出影响（粮食Val|粮食Per|金矿Val|金矿Per）
    /// </summary>
    public static readonly short[] TILE_HILLS_OUTPUT_EFFECT;
    /// <summary>
    /// 平原地形对产出影响（粮食Val|粮食Per|金矿Val|金矿Per）
    /// </summary>
    public static readonly short[] TILE_PlAIN_OUTPUT_EFFECT;
    /// <summary>
    /// 湖泊地形对产出影响（粮食Val|粮食Per|金矿Val|金矿Per）
    /// </summary>
    public static readonly short[] TILE_LAKES_OUTPUT_EFFECT;
    /// <summary>
    /// 山地对行军速度影响（士兵速度 = 初始速度 * value / 10000）
    /// </summary>
    public static readonly ushort TILE_MOUNTAIN_WALKABLE;
    /// <summary>
    /// 丘陵对行军速度影响（士兵速度 = 初始速度 * value / 10000）
    /// </summary>
    public static readonly ushort TILE_HILLS_WALKABLE;
    /// <summary>
    /// 平原对行军速度影响（士兵速度 = 初始速度 * value / 10000）
    /// </summary>
    public static readonly ushort TILE_PlAIN_WALKABLE;
    /// <summary>
    /// 湖泊对行军速度影响（士兵速度 = 初始速度 * value / 10000）
    /// </summary>
    public static readonly ushort TILE_LAKES_WALKABLE;
    #endregion

    #region Building
    /// <summary>
    /// 主城建筑范围(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_MAINBASE_AREA;
    /// <summary>
    /// 主城建筑中心点(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_MAINBASE_OFFSET;
    /// <summary>
    /// 田地建筑范围(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_FARMLAND_AREA;
    /// <summary>
    /// 田地建筑中心点(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_FARMLAND_OFFSET;
    /// <summary>
    /// 矿场建筑范围(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_GOLDMINE_AREA;
    /// <summary>
    /// 矿场建筑中心点(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_GOLDMINE_OFFSET;
    /// <summary>
    /// 兵营建筑范围(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_MILITARYCAMP_AREA;
    /// <summary>
    /// 兵营建筑中心点(X|Y)
    /// </summary>
    public static readonly byte[] BUILDING_MILITARYCAMP_OFFSET;
    /// <summary>
    /// 主城粮食产出间隔（秒）
    /// </summary>
    public static readonly float BUILDING_MAINBASE_GRAIN_INTERVAL;
    /// <summary>
    /// 主城金矿产出间隔（秒）
    /// </summary>
    public static readonly float BUILDING_MAINBASE_GOLD_INTERVAL;
    /// <summary>
    /// 主城士兵训练间隔（秒）
    /// </summary>
    public static readonly float BUILDING_MAINBASE_TRAIN_INTERVAL;
    /// <summary>
    /// 主城金矿产量
    /// </summary>
    public static readonly ushort BUILDING_MAINBASE_GOLD_OUTPUT;
    /// <summary>
    /// 主城粮食产量
    /// </summary>
    public static readonly ushort BUILDING_MAINBASE_GRAIN_OUTPUT;
    /// <summary>
    /// 主城士兵产量
    /// </summary>
    public static readonly ushort BUILDING_MAINBASE_SOLDIER_OUTPUT;
    /// <summary>
    /// 田地粮食产量
    /// </summary>
    public static readonly ushort BUILDING_FARMLAND_OUTPUT;
    /// <summary>
    /// 矿场金矿产量
    /// </summary>
    public static readonly ushort BUILDING_GOLDMINE_OUTPUT;
    /// <summary>
    /// 兵营士兵产量
    /// </summary>
    public static readonly ushort BUILDING_MILITARYCAMP_OUTPUT;
    /// <summary>
    /// 主城占领倒计时（秒）
    /// </summary>
    public static readonly float BULDING_MAINBASE_OCCUPIED_TIME;
    /// <summary>
    /// 田地创建花费（粮食|金矿）
    /// </summary>
    public static readonly ushort[] BUILDING_FARMLAND_CREATE_COST;
    /// <summary>
    /// 金矿创建花费（粮食|金矿）
    /// </summary>
    public static readonly ushort[] BUILDING_GOLDMINE_CREATE_COST;
    /// <summary>
    /// 兵营创建花费（粮食|金矿）
    /// </summary>
    public static readonly ushort[] BUILDING_MILITARYCAMP_CREATE_COST;
    /// <summary>
    /// 分城创建花费
    /// </summary>
    public static readonly ushort[] BUILDING_SUBBASE_CREATE_COST;
    /// <summary>
    /// 分城金矿产量
    /// </summary>
    public static readonly ushort BUILDING_SUBBASE_GOLD_OUTPUT;
    /// <summary>
    /// 分城粮食产量
    /// </summary>
    public static readonly ushort BUILDING_SUBBASE_GRAIN_OUTPUT;
    /// <summary>
    /// 分城士兵产量
    /// </summary>
    public static readonly ushort BUILDING_SUBBASE_SOLDIER_OUTPUT;
    /// <summary>
    /// 兵营增加的士兵储量上限
    /// </summary>
    public static readonly ushort BUILDING_MILITARYCAMP_SOLDIER_LIMIT;
    /// <summary>
    /// 田地增加的粮食储量上限
    /// </summary>
    public static readonly ushort BUILDING_FARMLAND_GRAIN_LIMIT;
    /// <summary>
    /// 矿场增加的金矿储量上限
    /// </summary>
    public static readonly ushort BUILDING_GOLDMINE_GOLD_LIMIT;
    /// <summary>
    /// 城与城建造时的最小距离
    /// </summary>
    public static readonly ushort BUILDING_MAINBASE_BUILD_MIN_LENGTH;

    #endregion

    #region Player
    /// <summary>
    /// 玩家初始粮食
    /// </summary>
    public static readonly ushort PLAYER_ORIGINAL_GRAIN;
    /// <summary>
    /// 玩家初始金矿
    /// </summary>
    public static readonly ushort PLAYER_ORIGINAL_GOLD;
    /// <summary>
    /// 玩家初始士兵量（放进第一个主城）
    /// </summary>
    public static readonly ushort PLAYER_ORIGINAL_SOLDIER;
    #endregion

    #region Soldier
    /// <summary>
    /// 士兵的预制体名称
    /// </summary>
    public static readonly string SOLDIER_PREFABE_NAME;
    /// <summary>
    /// 士兵的移动速度（米/秒，地图上每格子记为1米）
    /// </summary>
    public static readonly float SOLDIER_MOVE_SPEED;
    /// <summary>
    /// 士兵创建花费（粮食|金矿）
    /// </summary>
    public static readonly ushort[] SOLDIER_CREATE_COST;
    #endregion

    #region Map
    /// <summary>
    /// 小型地图边长（方形）
    /// </summary>
    public static readonly ushort MAP_SMALL_LENGTH;
    /// <summary>
    /// 中型地图边长（方形）
    /// </summary>
    public static readonly ushort MAP_NORMAL_LENGTH;
    /// <summary>
    /// 巨型地图边长（方形）
    /// </summary>
    public static readonly ushort MAP_BIG_LENGTH;
    /// <summary>
    /// 巨型地图边长（方形）
    /// </summary>
    public static readonly ushort MAP_HUGE_LENGTH;
    #endregion

    #region Others
    //场景Tag定义
    //地块的父节点Tag名称
    public static readonly string TAG_TILE_PARENT_NAME = "_TagTileParent";
    //Hud的Tag名称
    public static readonly string TAG_HUD_PARENT_NAME = "_TagHudParent";
    //战斗场景的相机
    public static readonly string TAG_BATTLE_SCENE_CAMERA_NAME = "_TagBattleSceneCamera";
    //战斗场景建筑层Tag
    public static readonly string TAG_BUILDING_PARENT_NAME = "_TagBuildingParent";
    //战斗场景士兵层Tag
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
    //玩家颜色
    public static readonly List<Color> PLAYER_COLOR_LIST = new List<Color>()
    {
        Color.blue,
        Color.cyan,
        Color.gray,
        Color.green,
        Color.red,
        Color.yellow,
    };

    #endregion

    #region 消息定义

    //MVC Commad
    public const string Cmd_ExitGame = "Cmd_ExitGame";
    public const string Cmd_ReturnLogin = "Cmd_ReturnLogin";
    public const string Cmd_LoginGame = "Cmd_LoginGame";
    public const string Cmd_StartBattle = "Cmd_StartBattle";
    public const string Cmd_Construction = "Cmd_Construction";
    public const string Cmd_ShowConstruction = "Cmd_ShowConstruction";
    public const string Cmd_CancelConstruction = "Cmd_CancelConstruction";
    public const string Cmd_ConfirmConstruction = "Cmd_ConfirmConstruction";
    public const string Cmd_PickMainBase = "Cmd_PickMainBase";
    public const string Cmd_MoveTroops = "Cmd_MoveTroops";
    public const string Cmd_MoveCamera = "Cmd_MoveCamera";
    public const string Cmd_UpdateMainBase = "Cmd_UpdateMainBase";
    public const string Cmd_MainBaseChangeOwer = "Cmd_PlayerFaild";

    //MVC Msg
    public const string Msg_InitConstructionMediator = "Msg_InitConstructionMediator";
    public const string Msg_InitLoginMediator = "Msg_InitLoginMediator";
    public const string Msg_InitMainMediator = "Msg_InitMainMediator";
    public const string Msg_InitBattleCameraMediator = "Msg_InitBattleCameraMediator";
    public const string Msg_InitMobilizeTroopsMediator = "Msg_InitMobilizeTroopsMediator";
    public const string Msg_InitPlayerBattleInfoMediator = "Msg_InitPlayerBattleInfoMediator";
    public const string Msg_BuildBuilding = "Msg_BuildBuilding";
    public const string Msg_PickMainBase = "Msg_PickMainBase";
    public const string Msg_UsersPlayerCreated = "Msg_UsersPlayerCreated";
    public const string Msg_SetUsersPlayerBattleInfoDirty = "Msg_SetUsersPlayerBattleInfoDirty";
    public const string Msg_ChangeMainBaseLevelUpState = "Msg_ChangeMainBaseLevelUpState";
    public const string Msg_UpdateMainBase = "Msg_UpdateMainBase";
    public const string Msg_AdjustFocuses = "Msg_AdjustFocuses";
    public const string Msg_MoveCamera = "Msg_MoveCamera";
    public const string Msg_CameraFocusPoint = "Msg_CameraFocusPoint";
    public const string Msg_PlayerFaild = "Msg_PlayerFaild";
    public const string Msg_EndBattle = "Msg_EndBattle";
    public const string Msg_StartBattle = "Msg_StartBattle";
    public const string Msg_InitMiniMapMediator = "Msg_InitMiniMapMediator";


    #endregion

    #region UIForm定义及初始化Command
    public const string UI_ConstructionUIForm = "ConstructionUIForm";               //建造界面
    public const string UI_LoginUIForm = "LoginUIForm";                             //登录界面
    public const string UI_MainUIForm = "MainUIForm";                               //出兵信息界面
    public const string UI_MobilizeTroopsInfoUIForm = "MobilizeTroopsInfoUIForm";   //主界面
    public const string UI_PlayerBattleInfoUIForm = "PlayerBattleInfoUIForm";       //玩家战斗信息
    public const string UI_ErrorStringTipsUIForm = "ErrorStringTipsUIForm";         //错误信息弹窗
    public const string UI_VictoryUIForm = "VictoryUIForm";                         //胜利界面
    public const string UI_FailedUIForm = "FailedUIForm";                           //失败界面
    #endregion

    //初始化参数
    static GlobalSetting()
    {
        //初始化TILE信息
        TILE_MOUNTAIN_OUTPUT_EFFECT = StaticDataHelper.GetShortArrayGSV("TILE_MOUNTAIN_OUTPUT_EFFECT");
        TILE_HILLS_OUTPUT_EFFECT = StaticDataHelper.GetShortArrayGSV("TILE_HILLS_OUTPUT_EFFECT");
        TILE_PlAIN_OUTPUT_EFFECT = StaticDataHelper.GetShortArrayGSV("TILE_PlAIN_OUTPUT_EFFECT");
        TILE_LAKES_OUTPUT_EFFECT = StaticDataHelper.GetShortArrayGSV("TILE_LAKES_OUTPUT_EFFECT");
        TILE_MOUNTAIN_WALKABLE = StaticDataHelper.GetUshortGSV("TILE_MOUNTAIN_WALKABLE");
        TILE_HILLS_WALKABLE = StaticDataHelper.GetUshortGSV("TILE_HILLS__WALKABLE");
        TILE_PlAIN_WALKABLE = StaticDataHelper.GetUshortGSV("TILE_PlAIN_WALKABLE");
        TILE_LAKES_WALKABLE = StaticDataHelper.GetUshortGSV("TILE_LAKES_WALKABLE");

        //初始化建筑信息
        BUILDING_MAINBASE_AREA = StaticDataHelper.GetByteArrayGSV("BUILDING_MAINBASE_AREA");
        BUILDING_MAINBASE_OFFSET = StaticDataHelper.GetByteArrayGSV("BUILDING_MAINBASE_OFFSET");
        BUILDING_FARMLAND_AREA = StaticDataHelper.GetByteArrayGSV("BUILDING_FARMLAND_AREA");
        BUILDING_FARMLAND_OFFSET = StaticDataHelper.GetByteArrayGSV("BUILDING_FARMLAND_OFFSET");
        BUILDING_GOLDMINE_AREA = StaticDataHelper.GetByteArrayGSV("BUILDING_GOLDMINE_AREA");
        BUILDING_GOLDMINE_OFFSET = StaticDataHelper.GetByteArrayGSV("BUILDING_GOLDMINE_OFFSET");
        BUILDING_MILITARYCAMP_AREA = StaticDataHelper.GetByteArrayGSV("BUILDING_MILITARYCAMP_AREA");
        BUILDING_MILITARYCAMP_OFFSET = StaticDataHelper.GetByteArrayGSV("BUILDING_MILITARYCAMP_OFFSET");
        BUILDING_MAINBASE_GRAIN_INTERVAL = StaticDataHelper.GetFloatGSV("BUILDING_MAINBASE_GRAIN_INTERVAL");
        BUILDING_MAINBASE_GOLD_INTERVAL = StaticDataHelper.GetFloatGSV("BUILDING_MAINBASE_GOLD_INTERVAL");
        BUILDING_MAINBASE_TRAIN_INTERVAL = StaticDataHelper.GetFloatGSV("BUILDING_MAINBASE_TRAIN_INTERVAL");
        BUILDING_MAINBASE_GOLD_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_MAINBASE_GOLD_OUTPUT");
        BUILDING_MAINBASE_GRAIN_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_MAINBASE_GRAIN_OUTPUT");
        BUILDING_MAINBASE_SOLDIER_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_MAINBASE_SOLDIER_OUTPUT");
        BUILDING_FARMLAND_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_FARMLAND_OUTPUT");
        BUILDING_GOLDMINE_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_GOLDMINE_OUTPUT");
        BUILDING_MILITARYCAMP_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_MILITARYCAMP_OUTPUT");
        BULDING_MAINBASE_OCCUPIED_TIME = StaticDataHelper.GetFloatGSV("BULDING_MAINBASE_OCCUPIED_TIME");
        BUILDING_FARMLAND_CREATE_COST = StaticDataHelper.GetUshortArrayGSV("BUILDING_FARMLAND_CREATE_COST");
        BUILDING_GOLDMINE_CREATE_COST = StaticDataHelper.GetUshortArrayGSV("BUILDING_GOLDMINE_CREATE_COST");
        BUILDING_SUBBASE_CREATE_COST = StaticDataHelper.GetUshortArrayGSV("BUILDING_SUBBASE_CREATE_COST");
        BUILDING_MILITARYCAMP_CREATE_COST = StaticDataHelper.GetUshortArrayGSV("BUILDING_MILITARYCAMP_CREATE_COST");
        BUILDING_SUBBASE_GOLD_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_SUBBASE_GOLD_OUTPUT");
        BUILDING_SUBBASE_GRAIN_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_SUBBASE_GRAIN_OUTPUT");
        BUILDING_SUBBASE_SOLDIER_OUTPUT = StaticDataHelper.GetUshortGSV("BUILDING_SUBBASE_SOLDIER_OUTPUT");
        BUILDING_MILITARYCAMP_SOLDIER_LIMIT = StaticDataHelper.GetUshortGSV("BUILDING_MILITARYCAMP_SOLDIER_LIMIT");
        BUILDING_FARMLAND_GRAIN_LIMIT = StaticDataHelper.GetUshortGSV("BUILDING_FARMLAND_GRAIN_LIMIT");
        BUILDING_GOLDMINE_GOLD_LIMIT = StaticDataHelper.GetUshortGSV("BUILDING_GOLDMINE_GOLD_LIMIT");
        BUILDING_MAINBASE_BUILD_MIN_LENGTH = StaticDataHelper.GetUshortGSV("BUILDING_MAINBASE_BUILD_MIN_LENGTH");



        //初始化Player信息
        PLAYER_ORIGINAL_GRAIN = StaticDataHelper.GetUshortGSV("PLAYER_ORIGINAL_GRAIN");
        PLAYER_ORIGINAL_GOLD = StaticDataHelper.GetUshortGSV("PLAYER_ORIGINAL_GOLD");
        PLAYER_ORIGINAL_SOLDIER = StaticDataHelper.GetUshortGSV("PLAYER_ORIGINAL_SOLDIER");

        //初始化士兵信息
        SOLDIER_PREFABE_NAME = StaticDataHelper.GetGlobalSettingValue("SOLDIER_PREFABE_NAME");
        SOLDIER_MOVE_SPEED = StaticDataHelper.GetFloatGSV("SOLDIER_MOVE_SPEED");
        SOLDIER_CREATE_COST = StaticDataHelper.GetUshortArrayGSV("SOLDIER_CREATE_COST");

        //初始化地图信息
        MAP_SMALL_LENGTH = StaticDataHelper.GetUshortGSV("MAP_SMALL_LENGTH");
        MAP_NORMAL_LENGTH = StaticDataHelper.GetUshortGSV("MAP_NORMAL_LENGTH");
        MAP_BIG_LENGTH = StaticDataHelper.GetUshortGSV("MAP_BIG_LENGTH");
        MAP_HUGE_LENGTH = StaticDataHelper.GetUshortGSV("MAP_HUGE_LENGTH");

        //Others
        //TAG_TILE_PARENT_NAME = "_TagTileParent";
        //TAG_BATTLE_SCENE_CAMERA_NAME = "_TagBattleSceneCamera";
        //TAG_BUILDING_PARENT_NAME = "_TagBuildingParent";
        //TAG_SOLDIERS_PARENT_NAME = "_TagSoldiersParent";
        //LAYER_MASK_NAME_GROUND = "Ground";
    }
}