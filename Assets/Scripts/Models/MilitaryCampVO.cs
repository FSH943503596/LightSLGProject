/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:21:04
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class MilitaryCampVO : IBuildingVO
{
    private int _SoldierNumLimit = 200;
    private float _TrainNum = 1;
    public MilitaryCampVO() {
        prefabName = "MilitaryCamp";
        _BuildingType = E_Building.MilitaryCamp;
    }

    public int soldierNumLimit { get => _SoldierNumLimit; set => _SoldierNumLimit = value; }
    public float trainNum { get => _TrainNum; set => _TrainNum = value; }
}

