/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:20:45
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public class FarmLandVO : IBuildingVO
{
    private float _GrainOutputNum = 1;
    private int _GrainLimit = 1000;
    public FarmLandVO()
    {
        prefabName = "FarmLand";
        _BuildingType = E_Building.FarmLand;
        SetOriginData();

    }

    public float grainOutputNum { get => _GrainOutputNum; set => _GrainOutputNum = value; }
    public int grainLimit { get => _GrainLimit; set => _GrainLimit = value; }

    public override ushort createCostGold => GlobalSetting.BUILDING_FARMLAND_CREATE_COST[1];
    public override ushort createCostGrain => GlobalSetting.BUILDING_FARMLAND_CREATE_COST[0];

    private void SetOriginData()
    {
        rect = new RectInt(GlobalSetting.BUILDING_FARMLAND_OFFSET[0],
                                          GlobalSetting.BUILDING_FARMLAND_OFFSET[1],
                                          GlobalSetting.BUILDING_FARMLAND_AREA[0],
                                          GlobalSetting.BUILDING_FARMLAND_AREA[1]);
        grainLimit = GlobalSetting.BUILDING_GOLDMINE_GOLD_LIMIT;
        grainOutputNum = GlobalSetting.BUILDING_GOLDMINE_OUTPUT;
    }
}

