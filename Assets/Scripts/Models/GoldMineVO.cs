/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:20:54
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class GoldMineVO:IBuildingVO
{
    private float _GoldOutputNum = 1;
    private int _GoldLimit = 1000;
    public GoldMineVO()
    {
        prefabName = "GoldMine";
        _BuildingType = E_Building.GoldMine;
    }

    public float goldOutputNum { get => _GoldOutputNum; set => _GoldOutputNum = value; }
    public int goldLimit { get => _GoldLimit; set => _GoldLimit = value; }
}

