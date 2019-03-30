/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:07:21
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using StaticData.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EMainBaseState
{
    None,
    Occupied,
    Occupying
}

public class MainBaseVO : IBuildingVO
{
    public static string SubPrefabName = "SubBase";

    private static List<MainBaseState> _MainBaseState = new List<MainBaseState>();

    private int _Radius = 4;
    private int _Level = 1;
    private List<IBuildingVO> _OwnBuildings = new List<IBuildingVO>();
    private PlayerVO _Ower;
    private bool _IsMain = false;
    private float _SoldierNum = 0;
    private int _SoldierNumLimit = 0;
    private float _TrainInterval= 999999;
    private float _TrainNum = 0;

    private float _GrainOutputNum = 0;
    private float _GrainOutputInterval = 999999;
    private int _GrainLimit = 0;

    private float _GoldOutputNum = 0;
    private float _GoldOutputInterval = 999999;
    private int _GoldLimit = 0;

    private MainBaseLevelData _Data;

    private MainBaseState _State;
    private bool _IsAllowMobilizeTroop = true;
    private bool _IsAllowBuildBuilding = true;
    private bool _IsAllowResetBuildingPosition = true;

    public MainBaseVO()
    {
        _Rect = new RectInt(0,0,1,1);
        prefabName = "MainBase";
        _BuildingType = E_Building.MainBase;
        _OwnBuildings.Add(this);
    }

    public MainBaseVO(PlayerVO ower) : this()
    {
        if (ower != null)
        {
            this._Ower = ower;
            _IsMain = ower.AddMainBases(this);
            if (!_IsMain) prefabName = SubPrefabName;
        }
    }

    public int radius { get => _Radius; set => _Radius = value; }
    public Vector3Int startTilePositonInMap => _TilePositon - new Vector3Int(_Radius - 1, 0, _Radius - 1);
    public bool isMain { get => _IsMain; set => _IsMain = value; }
    public List<IBuildingVO> ownBuildings  => _OwnBuildings; 
    public PlayerVO ower => _Ower;
    public override ushort createCostGold => GlobalSetting.BUILDING_SUBBASE_CREATE_COST[1];
    public override ushort createCostGrain => GlobalSetting.BUILDING_SUBBASE_CREATE_COST[0];
    public int soldierNum { get => (int)_SoldierNum; set => _SoldierNum = value; }
    public int soldierNumLimit { get => _SoldierNumLimit; set => _SoldierNumLimit = value; }
    public float trainInterval { get => _TrainInterval; set => _TrainInterval = value; }
    public float trainNum { get => _TrainNum; set => _TrainNum = value; }
    public float grainOutputNum { get => _GrainOutputNum; set => _GrainOutputNum = value; }
    public float grainOutputInterval { get => _GrainOutputInterval; set => _GrainOutputInterval = value; }
    public int grainLimit { get => _GrainLimit; set => _GrainLimit = value; }
    public float goldOutputNum { get => _GoldOutputNum; set => _GoldOutputNum = value; }
    public float goldOutputInterval { get => _GoldOutputInterval; set => _GoldOutputInterval = value; }
    public int goldLimit { get => _GoldLimit; set => _GoldLimit = value; }
    public MainBaseLevelData data { get => _Data; set => _Data = value; }

    public bool isSoldierBelowLimit => _SoldierNumLimit > _SoldierNum;

    public int level { get => _Level; set => _Level = value; }
    public bool isAllowMobilizeTroop { get => _IsAllowMobilizeTroop; set => _IsAllowMobilizeTroop = value; }
    public bool isAllowBuildBuilding { get => _IsAllowBuildBuilding; set => _IsAllowBuildBuilding = value; }
    public bool isAllowResetBuildingPosition { get => _IsAllowResetBuildingPosition; set => _IsAllowResetBuildingPosition = value; }

    public bool AddBuilding(IBuildingVO building)
    {
        if (building == null || building.prefabName.Equals("MainBase")) return false;

        if (_OwnBuildings.Contains(building)) return false;

        _OwnBuildings.Add(building);

        IncreateLimit(building);

        return true;
    }

    private void IncreateLimit(IBuildingVO building)
    {
        
    }

    public bool IsIn(Vector3Int pos)
    {
        return Mathf.Abs(pos.x - _TilePositon.x) + Mathf.Abs(pos.z - _TilePositon.z) < _Radius;
    }

    public void SetOwer(PlayerVO ower)
    {
        if (ower == null || ower.Equals(_Ower)) return;

        if (_Ower == null)
        {
            _Ower = ower;
        }
        else
        {
            _Ower.RemoveMainBaseVO(this);
            _Ower = ower;
        }

        ower.AddMainBases(this);
    }
}