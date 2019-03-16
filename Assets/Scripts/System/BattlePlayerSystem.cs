/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:59:32
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public class BattlePlayerSystem : IBattleSystem<BattleManager>
{
    private PlayerVOProxy _PlayerProxy;
    private BattleBuildingSystem _BuildingSystem;
    private BattleMapSystem _MapSystem;
    private PlayerVO _UserPlayer;
    private Vector3 _UserMainBasePosition;
    public BattlePlayerSystem(IBattleManager mgr) : base(mgr)
    {
        _BuildingSystem = battleManager.buildingSystem;
        _MapSystem = battleManager.mapSystem;
    }
    public Vector3 UserMainBasePosition { get => _UserMainBasePosition; }

    public void InitPlayers()
    {
        _PlayerProxy = facade.RetrieveProxy("PlayerProxy") as PlayerVOProxy;

        //创建玩家
        _UserPlayer = _PlayerProxy.CreatePlayer("我是玩家", 1);
        CreatePlayerFirstMainBase(_UserPlayer, 0.125f, 0.375f, 0.125f, 0.375f);
        _UserMainBasePosition = _UserPlayer.MainBases[0].postion;

        //创建敌军
        PlayerVO enemy = _PlayerProxy.CreatePlayer("我是敌人", 2);
        CreatePlayerFirstMainBase(enemy, 0.625f, 0.875f, 0.625f, 0.875f);

        //创建中立
        PlayerVO neutralPlayer;
        for (int i = 0; i < 2; i++)
        {
            neutralPlayer = _PlayerProxy.CreatePlayer("和气生财" + 1, 0);
            CreatePlayerFirstMainBase(neutralPlayer, 0.125f + i * 0.5f, 0.375f + i * 0.5f, 0.625f - i * 0.5f, 0.875f - i * 0.5f);
        }
    }

    private void CreatePlayerFirstMainBase(PlayerVO enemy, float startXPrec, float endXPrec, float startZPrec, float endZPrec)
    {
        //创建建筑数据对象
        MainBaseVO tempMainBaseVO = new MainBaseVO(enemy);
        //获取指定基地所在地图的位置
        tempMainBaseVO.tilePositon = _MapSystem.GetWalkableTileInRect(startXPrec, endXPrec, startZPrec, endZPrec);
        //将建筑放到地图中
        _MapSystem.AddBuildingInfo(tempMainBaseVO);
        //修改地块信息
        _MapSystem.ChangeMapTileInfos(tempMainBaseVO.startTilePositonInMap, GlobalSetting.MAINBASE_ORIGINAL_TILES);
        //显示建筑
        _BuildingSystem.CreateMainBase(tempMainBaseVO);
    }
}

