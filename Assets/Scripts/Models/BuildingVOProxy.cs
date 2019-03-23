/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-01-11:40:50
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Patterns;
using StaticData;
using StaticData.Data;
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

    public void CreateBuilding(IBuildingVO buildingVO, MainBaseVO mainBaseVO, PlayerVO player)
    {
        if (!_PlayerToMainBase.ContainsKey(player.Id)) _PlayerToMainBase.Add(player.Id, new List<MainBaseVO>());

        if (buildingVO.buildingType == E_Building.MainBase)
        {
            _PlayerToMainBase[player.Id].Add(mainBaseVO);
        }

        SetOriginData(buildingVO, mainBaseVO, player);

        _BuildingToMainBase.Add(buildingVO, mainBaseVO);

        SendNotification(GlobalSetting.Msg_BuildBuilding, buildingVO);

        if (player.IsUser) SendNotification(GlobalSetting.Msg_SetUsersPlayerBattleInfoDirty);
    }

    public void UpdateLevelMainBase(MainBaseVO mainBaseVO)
    {
        var currentData = mainBaseVO.data;
        var data = GetNextLevelDate(mainBaseVO);

        mainBaseVO.level++;
        mainBaseVO.data = data;
        mainBaseVO.radius = data.Radius;
        mainBaseVO.grainLimit += data.GrainLimit - currentData.GrainLimit;
        mainBaseVO.goldLimit += data.GoldLimit - currentData.GrainLimit;
        mainBaseVO.soldierNumLimit += data.SoldierLimit - currentData.SoldierLimit;

        mainBaseVO.ower.grainLimit += data.GrainLimit - currentData.GrainLimit;
        mainBaseVO.ower.goldLimit += data.GoldLimit - currentData.GrainLimit;
        mainBaseVO.soldierNumLimit += data.SoldierLimit - currentData.SoldierLimit;

        SendNotification(GlobalSetting.Msg_UpdateMainBase, mainBaseVO);
        SendNotification(GlobalSetting.Msg_SetUsersPlayerBattleInfoDirty);
    }

    private MainBaseLevelData GetNextLevelDate(MainBaseVO vO)
    {
        MainBaseLevelData data = vO.data;
        if (vO.isMain)
        {
            if (vO.level < StaticDataHelper.MainBaseMaxLevel)
            {
                data = StaticDataHelper.LevelToMainBaseLevelData[vO.level + 1];
            }
        }
        else
        {
            if (vO.level < StaticDataHelper.SubBaseMaxLevel)
            {
                data = StaticDataHelper.LevelToSubBaseLevelData[vO.level + 1];
            }
        }

        return data;
    }

    private void SetOriginData(IBuildingVO buildingVO, MainBaseVO mainBaseVO, PlayerVO player)
    {
        switch (buildingVO.buildingType)
        {
            case E_Building.MainBase:
                var tempMainBaseVO = buildingVO as MainBaseVO;
                tempMainBaseVO.SetOwer(player);
                SetMainBaseOriginData(tempMainBaseVO);
                player.goldLimit += tempMainBaseVO.goldLimit;
                player.grainLimit += tempMainBaseVO.grainLimit;
                player.soldierAmount += tempMainBaseVO.soldierNum;
                player.soldierAmountLimit += tempMainBaseVO.soldierNumLimit;
                break;
            case E_Building.FarmLand:
                var tempFarmLandVO = buildingVO as FarmLandVO;
                SetFarmLandOriginData(tempFarmLandVO);
                mainBaseVO.grainLimit += tempFarmLandVO.grainLimit;
                player.grainLimit += tempFarmLandVO.grainLimit;
                mainBaseVO.grainOutputNum += tempFarmLandVO.grainOutputNum;
                break;
            case E_Building.GoldMine:
                var tempGoldMineVO = buildingVO as GoldMineVO;
                SetGoldMineOriginData(tempGoldMineVO);
                mainBaseVO.goldLimit += tempGoldMineVO.goldLimit;
                player.goldLimit += tempGoldMineVO.goldLimit;
                mainBaseVO.goldOutputNum += tempGoldMineVO.goldOutputNum;
                break;
            case E_Building.MilitaryCamp:
                var tempMilitaryCampVO = buildingVO as MilitaryCampVO;
                SetMilitaryCampOriginData(tempMilitaryCampVO);
                mainBaseVO.soldierNumLimit += tempMilitaryCampVO.soldierNumLimit;
                player.soldierAmountLimit += tempMilitaryCampVO.soldierNumLimit;
                mainBaseVO.trainNum += tempMilitaryCampVO.trainNum;
                break;
            default:
                break;
        }
    }

    private void SetMilitaryCampOriginData(MilitaryCampVO militaryCampVO)
    {
        militaryCampVO.rect = new RectInt(GlobalSetting.BUILDING_MILITARYCAMP_OFFSET[0], 
                                          GlobalSetting.BUILDING_MILITARYCAMP_OFFSET[1],
                                          GlobalSetting.BUILDING_MILITARYCAMP_AREA[0],
                                          GlobalSetting.BUILDING_MILITARYCAMP_AREA[1]);
        militaryCampVO.soldierNumLimit = GlobalSetting.BUILDING_MILITARYCAMP_SOLDIER_LIMIT;
        militaryCampVO.trainNum = GlobalSetting.BUILDING_MILITARYCAMP_OUTPUT;
    }
    private void SetGoldMineOriginData(GoldMineVO goldMineVO)
    {
        goldMineVO.rect = new RectInt(GlobalSetting.BUILDING_GOLDMINE_OFFSET[0],
                                          GlobalSetting.BUILDING_GOLDMINE_OFFSET[1],
                                          GlobalSetting.BUILDING_GOLDMINE_AREA[0],
                                          GlobalSetting.BUILDING_GOLDMINE_AREA[1]);
        goldMineVO.goldLimit = GlobalSetting.BUILDING_GOLDMINE_GOLD_LIMIT;
        goldMineVO.goldOutputNum = GlobalSetting.BUILDING_GOLDMINE_OUTPUT;
    }
    private void SetFarmLandOriginData(FarmLandVO farmLandVO)
    {
        farmLandVO.rect = new RectInt(GlobalSetting.BUILDING_FARMLAND_OFFSET[0],
                                          GlobalSetting.BUILDING_FARMLAND_OFFSET[1],
                                          GlobalSetting.BUILDING_FARMLAND_AREA[0],
                                          GlobalSetting.BUILDING_FARMLAND_AREA[1]);
        farmLandVO.grainLimit = GlobalSetting.BUILDING_GOLDMINE_GOLD_LIMIT;
        farmLandVO.grainOutputNum = GlobalSetting.BUILDING_GOLDMINE_OUTPUT;
    }
    private void SetMainBaseOriginData(MainBaseVO mainBaseVO)
    {
        mainBaseVO.rect = new RectInt(GlobalSetting.BUILDING_MAINBASE_OFFSET[0],
                                          GlobalSetting.BUILDING_MAINBASE_OFFSET[1],
                                          GlobalSetting.BUILDING_MAINBASE_AREA[0],
                                          GlobalSetting.BUILDING_MAINBASE_AREA[1]);

        mainBaseVO.goldOutputInterval = GlobalSetting.BUILDING_MAINBASE_GOLD_INTERVAL;
        mainBaseVO.grainOutputInterval = GlobalSetting.BUILDING_MAINBASE_GRAIN_INTERVAL;
        mainBaseVO.trainInterval = GlobalSetting.BUILDING_MAINBASE_TRAIN_INTERVAL;

        if (mainBaseVO.isMain)
        {
            mainBaseVO.goldOutputNum = GlobalSetting.BUILDING_MAINBASE_GOLD_OUTPUT;
            mainBaseVO.grainOutputNum = GlobalSetting.BUILDING_MAINBASE_GRAIN_OUTPUT;
            mainBaseVO.trainNum = GlobalSetting.BUILDING_MAINBASE_SOLDIER_OUTPUT;
            mainBaseVO.data = StaticDataMgr.mInstance.mMainBaseLevelDataMap[10001];
            mainBaseVO.soldierNum = GlobalSetting.PLAYER_ORIGINAL_SOLDIER;
        }
        else
        {
            mainBaseVO.goldOutputNum = GlobalSetting.BUILDING_SUBBASE_GOLD_OUTPUT;
            mainBaseVO.grainOutputNum = GlobalSetting.BUILDING_SUBBASE_GRAIN_OUTPUT;
            mainBaseVO.trainNum = GlobalSetting.BUILDING_SUBBASE_SOLDIER_OUTPUT;
            mainBaseVO.data = StaticDataMgr.mInstance.mMainBaseLevelDataMap[20001];
            mainBaseVO.soldierNum = 0;
        }

        mainBaseVO.radius = mainBaseVO.data.Radius;
        mainBaseVO.grainLimit = mainBaseVO.data.GrainLimit;
        mainBaseVO.goldLimit = mainBaseVO.data.GoldLimit;
        mainBaseVO.soldierNumLimit = mainBaseVO.data.SoldierLimit;

        mainBaseVO.OccupiedMainBase(Time.time);
    }
    public MainBaseVO CreateMainBase(PlayerVO owner)
    {

        MainBaseVO mainBaseVO = new MainBaseVO(owner);

        CreateBuilding(mainBaseVO, mainBaseVO, owner);

        return mainBaseVO;
    }
    public void VisitMainBases(Action<MainBaseVO> mainbaseHandler)
    {
        if (mainbaseHandler == null) return;
        IEnumerator<MainBaseVO> enumerator = _BuildingToMainBase.Values.GetEnumerator();

        MainBaseVO temp;
        while (enumerator.MoveNext())
        {
            temp = enumerator.Current;

            mainbaseHandler(temp);
        }
    }
    public bool IsCanLevelUp(MainBaseVO vO)
    {
        bool isCan = false;

        if (vO.isMain)
        {
            isCan = vO.level < StaticDataHelper.MainBaseMaxLevel;
            
        }
        else
        {
            isCan = vO.level < StaticDataHelper.SubBaseMaxLevel;
        }

        isCan = isCan && vO.data.UpLevelGoldRequire <= vO.ower.gold;
        isCan = isCan && vO.data.UpLevelGrainRequire <= vO.ower.grain;

        return isCan;
    }

    public int GetMainbaseNextLevelRadius(MainBaseVO vO)
    {
        int radius = vO.radius;
        if (vO.isMain )
        {
            if (vO.level < StaticDataHelper.MainBaseMaxLevel)
            {
                radius = StaticDataHelper.LevelToMainBaseLevelData[vO.level + 1].Radius;
            }
        }
        else
        {
            if (vO.level < StaticDataHelper.SubBaseMaxLevel)
            {
                radius = StaticDataHelper.LevelToSubBaseLevelData[vO.level + 1].Radius;
            }
        }

        return radius;
    }
}

