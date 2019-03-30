/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-15:33:33
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public class BattleBuildingSystem : IBattleSystem<BattleManager>
{
    private BattleMapSystem _MapSystem;
    private BuildingVOProxy _BuildingVOProxy;
    public BattleBuildingSystem(IBattleManager battleMgr) : base(battleMgr)
    {
        _MapSystem = battleManager.mapSystem;
        _BuildingVOProxy = facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;
    }

    public void CreateMainBase(MainBaseVO vo)
    {
        _BuildingVOProxy.CreateBuilding(vo, vo, vo.ower);
    }

    public override void Update()
    {
        if (battleManager.isBattleOver) return;
        _BuildingVOProxy.UpdateMainBasesState(Time.time);
        facade.SendNotification(GlobalSetting.Msg_SetUsersPlayerBattleInfoDirty);
    }

    public override void Release()
    {
        base.Release();

        _BuildingVOProxy.Clear();
    }


    public bool IsMainBaseCanLevelUp(MainBaseVO vo)
    {
        return _BuildingVOProxy.IsCanLevelUp(vo);
    }

    public int GetNextLevelRadius(MainBaseVO vO)
    {
        return _BuildingVOProxy.GetMainbaseNextLevelRadius(vO);
    }
}

