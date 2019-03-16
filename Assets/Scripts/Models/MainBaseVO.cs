/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-22-17:07:21
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System.Collections.Generic;
using UnityEngine;

public class MainBaseVO : IBuildingVO
{
    public static string SubPrefabName = "SubBase";

    private int _Radius = 4;
    private List<IBuildingVO> _OwnBuildings = new List<IBuildingVO>();
    private PlayerVO _Ower;
    private bool _IsMain = false;
    private int _SoldierNum;
    private MainBaseState _State;
    private bool _IsAllowUpdate = true;
    private bool _IsAllowMobilizeTroop = true;
    private bool _IsAllowBuildBuilding = true;
    private bool _IsAllowResetBuildingPosition = true;

    public MainBaseVO()
    {
        _Rect = new RectInt(0, 0, 1, 1);
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

    public int soldierNum { get => _SoldierNum; set => _SoldierNum = value; }
    //public bool isAllowUpdate { get => _IsAllowUpdate; set => _IsAllowUpdate = value; }
    //public bool isAllowMobilizeTroop { get => _IsAllowMobilizeTroop; set => _IsAllowMobilizeTroop = value; }
    //public bool isAllowBuildBuilding { get => _IsAllowBuildBuilding; set => _IsAllowBuildBuilding = value; }
    //public bool isAllowResetBuildingPosition { get => _IsAllowResetBuildingPosition; set => _IsAllowResetBuildingPosition = value; }

    public bool AddBuilding(IBuildingVO building)
    {
        if (building == null || building.prefabName.Equals("MainBase")) return false;

        if (_OwnBuildings.Contains(building)) return false;

        _OwnBuildings.Add(building);

        return true;
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
            ower.AddMainBases(this);
            _Ower = ower;
        }
    }

    private abstract class MainBaseState
    {
        protected MainBaseVO _MainBaseVO;

        public virtual void Init(MainBaseVO mainBaseVO)
        {
            _MainBaseVO = mainBaseVO;
        }
        public abstract void Enter();
        public virtual void Update(float time) { }
    }

    private class OccupiedMainBaseState : MainBaseState
    {
        public override void Enter()
        {
            _MainBaseVO._IsAllowUpdate = true;
            _MainBaseVO._IsAllowMobilizeTroop = true;
            _MainBaseVO._IsAllowBuildBuilding = true;
            _MainBaseVO._IsAllowResetBuildingPosition = true;
        }
    }

    private class OccupyingMainBaseState : MainBaseState
    {
        private bool _IsReset = false;
        private float _StartTime = 0;

        public override void Enter()
        {
            _MainBaseVO._IsAllowUpdate = false;
            _MainBaseVO._IsAllowMobilizeTroop = false;
            _MainBaseVO._IsAllowBuildBuilding = false;
            _MainBaseVO._IsAllowResetBuildingPosition = false;
            _IsReset = true;
        }

        public override void Update(float time)
        {
            if(_IsReset)
            {
                _StartTime = time;
                _IsReset = false;
            }
        }
    }
}

