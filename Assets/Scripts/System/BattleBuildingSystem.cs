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
    private BattleMapSystem mapSystem;
    private BuildingVOProxy _BuildingVOProxy;
    public BattleBuildingSystem(IBattleManager battleMgr) : base(battleMgr)
    {
        mapSystem = battleManager.mapSystem;
        _BuildingVOProxy = facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;
    }

    public void CreateMainBase(MainBaseVO vo)
    {
        _BuildingVOProxy.CreateBuilding(vo, vo, vo.ower);
    }

    public override void Update()
    {
        _BuildingVOProxy.VisitMainBases(UpdateMainbaseVoHandler);

        facade.SendNotification(GlobalSetting.Msg_SetUsersPlayerBattleInfoDirty);
    }

    private void UpdateMainbaseVoHandler(MainBaseVO vo)
    {
        //主城数据自动更新
        vo.Update(Time.time);
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

