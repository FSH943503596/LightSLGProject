/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-15:33:33
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

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
        _BuildingVOProxy.CreateBuilding(vo, vo, vo.ower.Id);
    }
}

