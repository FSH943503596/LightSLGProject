/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-05-15:17:44
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class ConfirmConstructionCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        var msgParam = notification.Body as TwoMsgParams<PlayerVO, IBuildingVO>;
        
        if (msgParam != null && msgParam.first != null && msgParam.second != null)
        {
            PlayerVO playerVo = msgParam.first;
            IBuildingVO building = msgParam.second;
            PlayerVOProxy playerVOProxy = Facade.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy;
            MapVOProxy mapVOProxy = Facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;
            BuildingVOProxy buildingProxy = Facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;

            //判断资源是否满足创建需求
            if (playerVo.gold < building.createCostGold || playerVo.grain < building.createCostGrain)
            {
                return;
            }

            playerVo.gold -= building.createCostGold;
            playerVo.grain -= building.createCostGrain;

            switch (building.buildingType)
            {
                case E_Building.None:
                    break;
                case E_Building.MainBase:
                    //判断是否可以被添加....
                    MainBaseVO mainBaseVO = building as MainBaseVO;
                    if (mainBaseVO != null)
                    {
                        if (mapVOProxy.IsCanOccupedArea(mainBaseVO.tilePositon, mainBaseVO.radius))
                        {

                            mainBaseVO.SetOwer(playerVo);
                            buildingProxy.CreateBuilding(mainBaseVO, mainBaseVO, playerVo);
                            //更新地图建筑信息
                            mapVOProxy.SetBuildingInfo(true, building.tilePositon, building.rect);
                            //更新占领信息
                            mapVOProxy.SetOccupiedInfo(true, building.tilePositon, mainBaseVO.radius);
                        }
                    } 
                    break;
                case E_Building.FarmLand:
                case E_Building.GoldMine:
                case E_Building.MilitaryCamp:
                    MainBaseVO mainBase = playerVOProxy.GetMainBasePlayerBuildingBelongTo(building, playerVo);
                    if (mainBase != null)
                    {
                        //添加建筑
                        mainBase.AddBuilding(building);
                        //更新地图建筑信息
                        mapVOProxy.SetBuildingInfo(true, building.tilePositon, building.rect);
                        buildingProxy.CreateBuilding(building, mainBase, playerVo);
                    }               
                    break;
                default:
                    break;
            }

            TwoMsgParamsPool<PlayerVO, IBuildingVO>.Instance.Push(msgParam);
        }

        BattleManager.Instance.CancelConstraction();
    }
}

