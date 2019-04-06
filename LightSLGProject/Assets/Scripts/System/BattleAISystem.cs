/*
 *	Author:	贾树永
 *	CreateTime:	2019-04-02-09:55:59
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class BattleAISystem : IBattleSystem<BattleManager>
{
    public AIBuilder _AIBuilder;
    public AICommander _AICommander;

    public BattleAISystem(IBattleManager mgr) : base(mgr)
    {
        _AICommander = new AICommander();
        _AIBuilder = new AIBuilder();

        _AICommander.sender = facade.SendNotification;
        _AIBuilder.sender = facade.SendNotification;
        _AIBuilder.mapProxy = facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;
        _AIBuilder.buildingProxy = facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;
    }
}

