/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:21:04
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;

public class MilitaryCampVO : IBuildingVO
{
    private int _SoldierNumLimit = 200;
    private float _TrainNum = 1;
    public MilitaryCampVO() {
        prefabName = "MilitaryCamp";
        _BuildingType = E_Building.MilitaryCamp;
        SetMilitaryCampOriginData();
    }

    public int soldierNumLimit { get => _SoldierNumLimit; set => _SoldierNumLimit = value; }
    public float trainNum { get => _TrainNum; set => _TrainNum = value; }

    public override ushort createCostGold => GlobalSetting.BUILDING_MILITARYCAMP_CREATE_COST[1];
    public override ushort createCostGrain => GlobalSetting.BUILDING_MILITARYCAMP_CREATE_COST[0];

    private void SetMilitaryCampOriginData()
    {
        rect = new RectInt(GlobalSetting.BUILDING_MILITARYCAMP_OFFSET[0],
                                          GlobalSetting.BUILDING_MILITARYCAMP_OFFSET[1],
                                          GlobalSetting.BUILDING_MILITARYCAMP_AREA[0],
                                          GlobalSetting.BUILDING_MILITARYCAMP_AREA[1]);
        soldierNumLimit = GlobalSetting.BUILDING_MILITARYCAMP_SOLDIER_LIMIT;
        trainNum = GlobalSetting.BUILDING_MILITARYCAMP_OUTPUT;
    }
}

