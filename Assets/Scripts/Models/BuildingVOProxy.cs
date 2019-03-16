/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-01-11:40:50
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingVOProxy : Proxy
{
    new public const string NAME = "BuildingProxy";
    Dictionary<int, List<MainBaseVO>> _PlayerToMainBase = new Dictionary<int, List<MainBaseVO>>();
    Dictionary<IBuildingVO, MainBaseVO> _BuildingToMainBase = new Dictionary<IBuildingVO, MainBaseVO>();

    private IList<IBuildingVO> _Buildings
    {
        get
        {
            return base.m_data as IList<IBuildingVO>;
        }
    }

    public BuildingVOProxy() : base(NAME, new List<IBuildingVO>())
    {   
    }

    public void CreateBuilding(IBuildingVO buildingVO, MainBaseVO mainBaseVO, int playerID = 0)
    {
        if (!_PlayerToMainBase.ContainsKey(playerID)) _PlayerToMainBase.Add(playerID, new List<MainBaseVO>());

        if (buildingVO.buildingType == E_Building.MainBase)
        {
            _PlayerToMainBase[playerID].Add(mainBaseVO);
        }

        _BuildingToMainBase.Add(buildingVO, mainBaseVO);

        SendNotification(GlobalSetting.Msg_BuildBuilding, buildingVO);
    }

    public MainBaseVO CreateMainBase(PlayerVO owner)
    {

        MainBaseVO mainBaseVO = new MainBaseVO(owner);

        CreateBuilding(mainBaseVO, mainBaseVO, owner.Id);

        return mainBaseVO;
    }
}

