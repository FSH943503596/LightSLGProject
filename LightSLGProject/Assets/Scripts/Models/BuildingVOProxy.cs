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
    private Dictionary<int, List<MainBaseVO>> _PlayerToMainBase = new Dictionary<int, List<MainBaseVO>>();
    private Dictionary<IBuildingVO, MainBaseVO> _BuildingToMainBase = new Dictionary<IBuildingVO, MainBaseVO>();
    private List<MainBaseVO> _AllMainBases = new List<MainBaseVO>();
    private List<MainBaseState> _MainBaseStatesRecoverried = new List<MainBaseState>();
    private Dictionary<MainBaseVO, MainBaseState> _MainBaseStatesInUsed = new Dictionary<MainBaseVO, MainBaseState>();
    private IList<IBuildingVO> _Buildings
    {
        get
        {
            return base.m_data as IList<IBuildingVO>;
        }
    }

    internal void Clear()
    {
        _PlayerToMainBase.Clear();
        _BuildingToMainBase.Clear();
        _MainBaseStatesRecoverried.Clear();
        _MainBaseStatesInUsed.Clear();
        _AllMainBases.Clear();
    }

    public BuildingVOProxy() : base(NAME, new List<IBuildingVO>())
    {   
    }

    public int mainBaseTotalCount => _MainBaseStatesInUsed.Count;

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
                SetMainBaseOriginData(tempMainBaseVO);
                player.goldLimit += tempMainBaseVO.goldLimit;
                player.grainLimit += tempMainBaseVO.grainLimit;
                player.soldierAmount += tempMainBaseVO.soldierNum;
                player.soldierAmountLimit += tempMainBaseVO.soldierNumLimit;
                _AllMainBases.Add(tempMainBaseVO);
                OccupiedMainBase(tempMainBaseVO, Time.time);

                SendNotification(GlobalSetting.Msg_MainbaseCreateComplete, tempMainBaseVO);
                break;
            case E_Building.FarmLand:
                var tempFarmLandVO = buildingVO as FarmLandVO;
                //SetFarmLandOriginData(tempFarmLandVO);
                mainBaseVO.grainLimit += tempFarmLandVO.grainLimit;
                player.grainLimit += tempFarmLandVO.grainLimit;
                mainBaseVO.grainOutputNum += tempFarmLandVO.grainOutputNum;
                break;
            case E_Building.GoldMine:
                var tempGoldMineVO = buildingVO as GoldMineVO;
                //SetGoldMineOriginData(tempGoldMineVO);
                mainBaseVO.goldLimit += tempGoldMineVO.goldLimit;
                player.goldLimit += tempGoldMineVO.goldLimit;
                mainBaseVO.goldOutputNum += tempGoldMineVO.goldOutputNum;
                break;
            case E_Building.MilitaryCamp:
                var tempMilitaryCampVO = buildingVO as MilitaryCampVO;
                //SetMilitaryCampOriginData(tempMilitaryCampVO);
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
        if (mainBaseVO.isMain)
        {
            mainBaseVO.goldOutputNum = GlobalSetting.BUILDING_MAINBASE_GOLD_OUTPUT;
            mainBaseVO.grainOutputNum = GlobalSetting.BUILDING_MAINBASE_GRAIN_OUTPUT;
            mainBaseVO.trainNum = GlobalSetting.BUILDING_MAINBASE_SOLDIER_OUTPUT;
            mainBaseVO.data = StaticDataHelper.LevelToMainBaseLevelData[1];
            mainBaseVO.soldierNum = GlobalSetting.PLAYER_ORIGINAL_SOLDIER;
        }
        else
        {
            mainBaseVO.goldOutputNum = GlobalSetting.BUILDING_SUBBASE_GOLD_OUTPUT;
            mainBaseVO.grainOutputNum = GlobalSetting.BUILDING_SUBBASE_GRAIN_OUTPUT;
            mainBaseVO.trainNum = GlobalSetting.BUILDING_SUBBASE_SOLDIER_OUTPUT;
            mainBaseVO.data = StaticDataHelper.LevelToSubBaseLevelData[1];
            mainBaseVO.soldierNum = 0;
        }

        mainBaseVO.radius = mainBaseVO.data.Radius;
        mainBaseVO.grainLimit = mainBaseVO.data.GrainLimit;
        mainBaseVO.goldLimit = mainBaseVO.data.GoldLimit;
        mainBaseVO.soldierNumLimit = mainBaseVO.data.SoldierLimit;
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
    public void UpdateMainBasesState(float time)
    {
        var enumerator = _AllMainBases.GetEnumerator();
        MainBaseState mainBaseState;
        
        while (enumerator.MoveNext())
        {
            mainBaseState = _MainBaseStatesInUsed[enumerator.Current];

            if (mainBaseState.IsNeedChange(time))
            {
                mainBaseState = ChangeState(mainBaseState);
                mainBaseState.Enter(time);
            }
            else
            {
                mainBaseState.Update(time);
            }         
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
    private MainBaseState ChangeState(MainBaseState state)
    {
        MainBaseState newState;
        if (state.eMainBaseState == EMainBaseState.Occupied)
        {
            newState = GetState(EMainBaseState.Occupying) ?? state;
        }
        else
        {
            newState = GetState(EMainBaseState.Occupied) ?? state;
        }

        if (!state.Equals(newState))
        {
            newState.Init(state.mainBaseVO, state.occuptedPlayer);
            _MainBaseStatesRecoverried.Add(state);
            _MainBaseStatesInUsed[state.mainBaseVO] = newState;
        }

        return newState;
    }
    private MainBaseState GetState(EMainBaseState stateType)
    {
        MainBaseState state;
        state = _MainBaseStatesRecoverried.Find(s => s.eMainBaseState == stateType);
        if (state != null)
        {
            _MainBaseStatesRecoverried.Remove(state);
        }
        else
        {
            switch (stateType)
            {
                case EMainBaseState.None:
                    break;
                case EMainBaseState.Occupied:
                    state = new OccupiedMainBaseState(SendNotification);
                    break;
                case EMainBaseState.Occupying:
                    state = new OccupyingMainBaseState(SendNotification);
                    break;
                default:
                    break;
            }
        }

        return state;
    }
    public void ReceiveSoldier(MainBaseVO mainBaseVO, PlayerVO player, float time)
    {
        MainBaseState mainBaseState;
        if (_MainBaseStatesInUsed.TryGetValue(mainBaseVO, out mainBaseState))
        {
            mainBaseState.ReceiveSoldier(player, time);
        }      
    }
    private void OccupiedMainBase(MainBaseVO mainBaseVO, float time)
    {
        MainBaseState mainBaseState ;
        mainBaseState = GetState(EMainBaseState.Occupied);
        mainBaseState.Init(mainBaseVO, mainBaseVO.ower);
        mainBaseState.Enter(time);
        _MainBaseStatesInUsed.Add(mainBaseVO, mainBaseState);
    }
}