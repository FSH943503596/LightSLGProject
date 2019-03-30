/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:20:54
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public class GoldMineVO:IBuildingVO
{
    private float _GoldOutputNum = 1;
    private int _GoldLimit = 1000;
    public GoldMineVO()
    {
        prefabName = "GoldMine";
        _BuildingType = E_Building.GoldMine;
        SetOriginData();
    }

    public float goldOutputNum { get => _GoldOutputNum; set => _GoldOutputNum = value; }
    public int goldLimit { get => _GoldLimit; set => _GoldLimit = value; }
    public override ushort createCostGold => GlobalSetting.BUILDING_GOLDMINE_CREATE_COST[1];
    public override ushort createCostGrain => GlobalSetting.BUILDING_GOLDMINE_CREATE_COST[0];

    private void SetOriginData()
    {
        rect = new RectInt(GlobalSetting.BUILDING_GOLDMINE_OFFSET[0],
                                          GlobalSetting.BUILDING_GOLDMINE_OFFSET[1],
                                          GlobalSetting.BUILDING_GOLDMINE_AREA[0],
                                          GlobalSetting.BUILDING_GOLDMINE_AREA[1]);
        goldLimit = GlobalSetting.BUILDING_GOLDMINE_GOLD_LIMIT;
        goldOutputNum = GlobalSetting.BUILDING_GOLDMINE_OUTPUT;
    }
}

