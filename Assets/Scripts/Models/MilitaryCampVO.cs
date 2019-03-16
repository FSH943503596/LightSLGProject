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
    public MilitaryCampVO() {
        prefabName = "MilitaryCamp";
        _BuildingType = E_Building.MilitaryCamp;
    }
}

