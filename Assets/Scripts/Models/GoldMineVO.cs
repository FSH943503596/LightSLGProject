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
    public GoldMineVO()
    {
        prefabName = "GoldMine";
        _BuildingType = E_Building.GoldMine;
    }
}

