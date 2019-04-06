/*
 *	Author:	贾树永
 *	CreateTime:	2019-04-02-13:57:15
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class AIBuilder
{
    public Action<string, object> sender;
    public MapVOProxy mapProxy;
    public BuildingVOProxy buildingProxy;

    private Func<ConstructionInfo[], int, int, ConstructionInfo> _SelectConstructionHandler;
    private ConstructionInfo[] _ConstructionInfos;
    private int _MainbaseMaxRadius;
    private int _SubbaseMaxRadius;

    public AIBuilder()
    {
        _SelectConstructionHandler = SelectConstructionInfoByRandom;
        _ConstructionInfos = new ConstructionInfo[20];
    }
    public void Build(PlayerVO ower, E_Building buildingType, MainBaseVO mainBaseVO)
    {
        IBuildingVO buildingVO = null ;
        Vector3Int center = default ;
        int radius = 0;
        int count = 0;
        if (mainBaseVO != null)
        {
            center = mainBaseVO.tilePositon;
            radius = mainBaseVO.radius;
        }
        switch (buildingType)
        {
            case E_Building.None:
                break;
            case E_Building.MainBase:
                buildingVO = new MainBaseVO();
                count = GetBuildMainbasePositionClose(_ConstructionInfos, ower);
                break;
            case E_Building.FarmLand:
                buildingVO = new FarmLandVO();
                count = GetBuildPosition(_ConstructionInfos, center, radius, buildingVO.rect);
                break;
            case E_Building.GoldMine:
                buildingVO = new GoldMineVO();
                count = GetBuildPosition(_ConstructionInfos, center, radius, buildingVO.rect);
                break;
            case E_Building.MilitaryCamp:
                buildingVO = new MilitaryCampVO();
                count = GetBuildPosition(_ConstructionInfos, center, radius, buildingVO.rect);
                break;
            default:
                break;
        }
        ConstructionInfo constructionInfo = _SelectConstructionHandler(_ConstructionInfos, count, 0);
        if(constructionInfo.isRotation) buildingVO.Rotate();
        buildingVO.tilePositon = constructionInfo.position;
        var msgParam = TwoMsgParamsPool<PlayerVO, IBuildingVO>.Instance.Pop();
        msgParam.InitParams(ower, buildingVO);
        sender.Invoke(GlobalSetting.Cmd_ConfirmConstruction, msgParam);
    }
    public void UpgradeMainbase(MainBaseVO mainBaseVO)
    {
        if (mainBaseVO == null) return;
        if (buildingProxy.IsCanLevelUp(mainBaseVO))
        {
            sender(GlobalSetting.Cmd_UpdateMainBase, mainBaseVO);
        }
    }
    private int GetBuildPosition(ConstructionInfo[] outPositionList, Vector3Int center, int radius, RectInt buildingRect)
    {
        if (outPositionList == null && outPositionList.Length == 0) return 0;

        int count = 0;

        for (int x = 1 - radius; x < radius; x++)
        {
            for (int z = 1-radius; z < radius; z++)
            {
                //判断矩形是否在半径内   
                if (Math.Abs(x) + Math.Abs(z) >= radius) continue;
                if (Math.Abs(x + buildingRect.width - 1) + Math.Abs(z + buildingRect.height - 1) < radius)
                {
                    //判断区域内是否有建筑
                    if (IsBlackRect(center + new Vector3Int(x, 0, z), buildingRect.width, buildingRect.height))
                    {
                        outPositionList[count].position = center + new Vector3Int(buildingRect.position.x + x, 0, buildingRect.position.y + z);
                        outPositionList[count].isRotation = false;
                        //TODO 增加影响信息
                        count++;
                        if (count >= outPositionList.Length) break;
                    }
                }

                if (Math.Abs(x + buildingRect.height - 1) + Math.Abs(z + buildingRect.width - 1) < radius)
                {
                    //判断区域内是否有建筑
                    if (IsBlackRect(center + new Vector3Int(x, 0, z), buildingRect.height, buildingRect.width))
                    {
                        outPositionList[count].position = center + new Vector3Int(buildingRect.height - buildingRect.position.y - 1 + x, 0, buildingRect.position.x + z);
                        outPositionList[count].isRotation = true;
                        //TODO 增加影响信息
                        count++;
                        if (count >= outPositionList.Length) break;
                    }
                }
            }
            if (count >= outPositionList.Length) break;
        }

        return count;
    }
    private bool IsBlackRect(Vector3Int vector3Int, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (mapProxy.IsBuilding(vector3Int.x + i, vector3Int.z + j)) return false;
            }
        }

        return true;
    }
    private int GetBuildMainbasePositionClose(ConstructionInfo[] outPositionList, PlayerVO ower)
    {
        if (outPositionList == null && outPositionList.Length == 0) return 0;

        int count = 0;

        MainBaseVO closeMainbase = null;
        MainBaseVO testMainBase = null;
        int radius = 0;
        int testRadius = 0;
        int absY = 0;
        int length = 0;
        for (int i = 0; i < ower.mainBases.Count; i++)
        {
            closeMainbase = ower.mainBases[i];

            radius = closeMainbase.isMain ? _MainbaseMaxRadius : _SubbaseMaxRadius;
            radius += _SubbaseMaxRadius;
            bool isNeedAdd = true;
            for (int x = 1 - radius; x < radius; x++)
            {
                absY = (radius - 1) - Math.Abs(x);

                Vector3Int temp = testMainBase.tilePositon + new Vector3Int(x, 0, absY);
                for (int j = 0; j < ower.mainBases.Count; j++)
                {
                    if (i == j) continue;
                    testMainBase = ower.mainBases[j];
                    isNeedAdd = true;
                    length = Math.Abs(temp.x - testMainBase.tilePositon.x) + Math.Abs(temp.z - testMainBase.tilePositon.z) + 1;
                    if (length >= radius)
                    {
                        isNeedAdd = false;
                        break;
                    }
                }
                
                
                if (mapProxy.IsCanOccupedArea(temp, _SubbaseMaxRadius))
                {
                    isNeedAdd = true;
                    for (int k = 0; k < count; k++)
                    {
                        if (temp == outPositionList[k].position)
                        {
                            isNeedAdd = false;
                            break;
                        }
                    }
                    if (isNeedAdd)
                    {
                        outPositionList[count].position = temp;
                        outPositionList[count].isRotation = false;
                        //TODO 获取城堡坐在的位置的资源加成数据
                        count++;
                        if (count >= outPositionList.Length) return count;
                    }
                }
                temp = testMainBase.tilePositon + new Vector3Int(x, 0, -absY);
                if (mapProxy.IsCanOccupedArea(temp, _SubbaseMaxRadius))
                {
                    isNeedAdd = true;
                    for (int k = 0; k < count; k++)
                    {
                        if (temp == outPositionList[k].position)
                        {
                            isNeedAdd = false;
                            break;
                        }
                    }
                    if (isNeedAdd)
                    {
                        outPositionList[count].position = temp;
                        outPositionList[count].isRotation = false;
                        //TODO 获取城堡坐在的位置的资源加成数据

                        count++;
                        if (count >= outPositionList.Length) return count;
                    }
                }
            }    
        }

        return count;
    }
    private ConstructionInfo SelectConstructionInfoByRandom(ConstructionInfo[] constructionArray, int count, int fromIndex = 0)
    {
        //获取集合中的随机值
        int index = UnityEngine.Random.Range(fromIndex, fromIndex + count - 1);

        return constructionArray[index];
    }
    private ConstructionInfo SelectConstructionInfoByGreedy(ConstructionInfo constructionArray, int count, int fromIndex = 0)
    {
        //TODO 获取加成最大者

        return default;
    }
    private struct ConstructionInfo
    {
        public Vector3Int position;
        public bool isRotation;
        public float goldNumAdd;
        public float goldPerAdd;
        public float grainNumAdd;
        public float grainPerAdd;
    }
}

