/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:20:45
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class FarmLandVO : IBuildingVO
{
    public FarmLandVO()
    {
        prefabName = "FarmLand";
        _BuildingType = E_Building.FarmLand;
    }
}

