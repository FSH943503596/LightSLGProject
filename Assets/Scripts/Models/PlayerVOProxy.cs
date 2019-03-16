/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-25-11:54:55
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVOProxy : Proxy
{
    new public const string NAME = "PlayerProxy";

    private IList<PlayerVO> _Players
    {
        get
        {
            return base.Data as IList<PlayerVO>;
        }
    }

    public PlayerVOProxy() : base(NAME, new List<PlayerVO>())
    {        
    }

    /// <summary>
    /// 创建一个玩家
    /// </summary>
    /// <param name="name">玩家名称</param>
    /// <param name="group">玩家属于的阵营</param>
    /// <returns></returns>
    public PlayerVO CreatePlayer(string name, byte group)
    {
        PlayerVO player = new PlayerVO(name, group);

        _Players.Add(player);

        return player;
    }

    public void ClearPlayers()
    {
        _Players.Clear();
    }

    /// <summary>
    /// 获取玩家的主基地列表
    /// </summary>
    /// <returns></returns>
    public IList<MainBaseVO> GetUsersMainBaseList()
    {
        for (int i = 0; i < _Players.Count; i++)
        {
            if (_Players[i].Id == 1) return _Players[i].MainBases;
        }

        return null;
    }

    /// <summary>
    /// 判断建筑是否能够建造
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="building"></param>
    /// <returns></returns>
    public bool IsCanConstructionUserBuilding(int playerID, IBuildingVO building)
    {
        PlayerVO player = _Players[playerID];
        IList<MainBaseVO> mainbases = player.MainBases;
        List<IBuildingVO> buildings = null;
        Vector3Int downLeft;
        Vector3Int middle;
        Vector3Int upRight;
        Vector3Int downLeftb;
        Vector3Int upRightb;

        if (mainbases.Count == 0) return false;

        for (int i = 0; i < mainbases.Count; i++)
        {
            downLeft = building.tilePositon - new Vector3Int(building.rect.position.x, 0, building.rect.position.y);
            middle = downLeft;
            if (!mainbases[i].IsIn(middle)) continue;
            middle.x += building.rect.size.x - 1;
            if (!mainbases[i].IsIn(middle)) continue;
            middle.z += building.rect.size.y - 1;
            upRight = middle;
            if (!mainbases[i].IsIn(middle)) continue;
            middle.x -= building.rect.size.x - 1;
            if (!mainbases[i].IsIn(middle)) continue;

            buildings = mainbases[i].ownBuildings;
            for (int j = 0; j < buildings.Count; j++)
            {
                downLeftb = buildings[j].tilePositon - new Vector3Int(building.rect.position.x, 0, building.rect.position.y);
                upRightb = downLeftb + new Vector3Int(buildings[j].rect.width - 1, 0, buildings[j].rect.height - 1);
                if (downLeft.x <= upRightb.x && upRight.x >= downLeftb.x && downLeft.z <= upRightb.z && upRight.z >= downLeftb.z)
                {
                    return false;
                }
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// 获取玩家数据
    /// </summary>
    /// <returns></returns>
    public PlayerVO GetUserVO()
    {
        return _Players[0];
    }

    /// <summary>
    /// 获取建筑属于玩家的哪个基地，不属于，返回NULL
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public MainBaseVO GetMainBaseUserBuildingBelongTo(IBuildingVO building)
    {
        if (building.buildingType == E_Building.MainBase) return building as MainBaseVO;

        PlayerVO player = GetUserVO();
        IList<MainBaseVO> mainbases = player.MainBases;
        List<IBuildingVO> buildings = null;
        Vector3Int downLeft;
        Vector3Int upRight;
        Vector3Int downLeftb;
        Vector3Int upRightb;

        if (mainbases.Count == 0) return null;

        for (int i = 0; i < mainbases.Count; i++)
        {
            downLeft = building.tilePositon - new Vector3Int(building.rect.position.x, 0, building.rect.position.y);
            if (!mainbases[i].IsIn(downLeft)) continue;
            downLeft.x += building.rect.size.x - 1;
            if (!mainbases[i].IsIn(downLeft)) continue;
            downLeft.z += building.rect.size.y - 1;
            upRight = downLeft;
            if (!mainbases[i].IsIn(downLeft)) continue;
            downLeft.x -= building.rect.size.x - 1;
            if (!mainbases[i].IsIn(downLeft)) continue;

            buildings = mainbases[i].ownBuildings;
            for (int j = 0; j < buildings.Count; j++)
            {
                downLeftb = buildings[j].tilePositon - new Vector3Int(building.rect.position.x, 0, building.rect.position.y);
                upRightb = downLeftb + new Vector3Int(buildings[j].rect.width - 1, 0, buildings[j].rect.height - 1);
                if (downLeft.x <= upRightb.x && upRight.x >= downLeftb.x && downLeft.z <= upRightb.z && upRight.z >= downLeftb.z)
                {
                    return null;
                }
            }
            return mainbases[i];
        }

        return null;
    }
}

