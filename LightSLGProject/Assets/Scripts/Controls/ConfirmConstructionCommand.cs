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
        IBuildingVO building = notification.Body as IBuildingVO;
        

        if (building != null)
        {
            PlayerVOProxy playerVOProxy = Facade.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy;
            
            MapVOProxy mapVOProxy = Facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;

            BuildingVOProxy buildingProxy = Facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;

            PlayerVO userVo = playerVOProxy.GetUserVO();

            //判断资源是否满足创建需求
            if (userVo.gold < building.createCostGold || userVo.grain < building.createCostGrain)
            {
                return;
            }

            userVo.gold -= building.createCostGold;
            userVo.grain -= building.createCostGrain;

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

                            mainBaseVO.SetOwer(userVo);
                            buildingProxy.CreateBuilding(mainBaseVO, mainBaseVO, userVo);
                            //userVo.AddMainBases(building as MainBaseVO);
                            //更新地图建筑信息
                            mapVOProxy.SetBuildingInfo(true, building.tilePositon, building.rect);
                            //更新占领信息
                            mapVOProxy.SetOccupiedInfo(true, building.tilePositon, mainBaseVO.radius);
                            //buildingProxy.CreateBuilding(mainBaseVO, mainBaseVO, userVo);

                            //mainBaseVO.SetOwer(userVo);
                        }
                    } 
                    break;
                case E_Building.FarmLand:
                case E_Building.GoldMine:
                case E_Building.MilitaryCamp:
                    MainBaseVO mainBase = playerVOProxy.GetMainBaseUserBuildingBelongTo(building);
                    if (mainBase != null)
                    {
                        //添加建筑
                        mainBase.AddBuilding(building);
                        //更新地图建筑信息
                        mapVOProxy.SetBuildingInfo(true, building.tilePositon, building.rect);
                        buildingProxy.CreateBuilding(building, mainBase, userVo);
                    }               
                    break;
                default:
                    break;
            }
        }

        BattleManager.Instance.CancelConstraction();
    }
}

