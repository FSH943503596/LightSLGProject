/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-22-11:22:41
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class UpdateMainBaseCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        MainBaseVO mainBaseVO = notification.Body as MainBaseVO;

        if (mainBaseVO != null)
        {
            var buildingProxy = Facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;
            var mapProxy = Facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;

            bool isCanLevelUp = buildingProxy.IsCanLevelUp(mainBaseVO);
            int nextLevelRadius = buildingProxy.GetMainbaseNextLevelRadius(mainBaseVO);
            isCanLevelUp = isCanLevelUp && mapProxy.IsCanOccupedRingArea(mainBaseVO.tilePositon, mainBaseVO.radius, nextLevelRadius);
            if (isCanLevelUp)
            {
                //扣除消耗
                mainBaseVO.ower.gold -= mainBaseVO.data.UpLevelGoldRequire;
                mainBaseVO.ower.grain -= mainBaseVO.data.UpLevelGrainRequire;
                //占用地图
                mapProxy.SetOccupiedInfo(true, mainBaseVO.tilePositon, nextLevelRadius);
                //扩大范围
                buildingProxy.UpdateLevelMainBase(mainBaseVO);
            }
        }
    }
}

