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

    public void OccupiedMainBase(float time)
    {
        if (_State == null)
        {
            _State = GetState(EMainBaseState.Occupied);
        }

        _State.Init(this, this.ower.Id);
        _State.Enter(time);
    }

    private static MainBaseState GetState(EMainBaseState stateType)
    {
        MainBaseState state;
        state = _MainBaseState.Find(s => s.eMainBaseState == stateType);
        if (state == null)
        {
            switch (stateType)
            {
                case EMainBaseState.None:
                    break;
                case EMainBaseState.Occupied:
                    state = new OccupiedMainBaseState();
                    break;
                case EMainBaseState.Occupying:
                    state = new OccupyingMainBaseState();
                    break;
                default:
                    break;
            }
        }

        return state;
    }

    public void OccupingMainBase(float time)
    {

    }

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
            ower.AddMainBases(this);
            _Ower = ower;
        }
    }

    public void Update(float time)
    {
        if (!_State.isMine)
        {
            ChangeState(_State);
            _State.Enter(time);
        }
        _State.Update(time);
    }

    public void ReceiveSoldier(PlayerVO player, float time)
    {
        _State.ReceiveSoldier(player, time);
    }

    private void ChangeState(MainBaseState state)
    {
        if (state.eMainBaseState == EMainBaseState.Occupied)
        {
            _State = GetState(EMainBaseState.Occupying) ?? state;
        }
        else
        {
            _State = GetState(EMainBaseState.Occupied) ?? state;
        }

        if (!state.Equals(_State))
        {
            _State.Init(this, state.occuptedPlayerID);
            _MainBaseState.Add(state);
        }
    }

    private abstract class MainBaseState
    {
        protected MainBaseVO _MainBaseVO;
        protected float startTime = 0;
        protected EMainBaseState _EMainBaseState = EMainBaseState.None;
        protected int _OccuptedPlayerID;

        public bool isMine => _OccuptedPlayerID == _MainBaseVO.ower.Id;
        public EMainBaseState eMainBaseState => eMainBaseState;

        public int occuptedPlayerID { get => _OccuptedPlayerID; set => _OccuptedPlayerID = value; }

        public virtual void Init(MainBaseVO mainBaseVO, int occupiedPlayer)
        {
            _MainBaseVO = mainBaseVO;
            _OccuptedPlayerID = occupiedPlayer;
        }
        public virtual void Enter(float time)
        {
            startTime = time; 
        }
        public virtual void Update(float time) { }

        public virtual void ReceiveSoldier(PlayerVO player, float time)
        {
            if (_OccuptedPlayerID != player.Id)
            {
                _MainBaseVO.soldierNum--;
                _MainBaseVO.ower.soldierAmount--;
                player.soldierAmount -= 1;
            }
            else
            {
                _MainBaseVO.soldierNum++;
            }
        }

        protected void ChangePlayer(PlayerVO player)
        {
            if (_MainBaseVO.soldierNum < 0)
            {
                _OccuptedPlayerID = player.Id;
                _MainBaseVO.soldierNum *= -1;
            }
        }
    }

    private class OccupiedMainBaseState : MainBaseState
    {
        private float _NextTrainSoldierTime = 0;
        private float _NextCreateGrainTime = 0;
        private float _NextCreatGoldTime = 0;

        public OccupiedMainBaseState()
        {
            _EMainBaseState = EMainBaseState.Occupied;
        }

        public override void Enter(float time)
        {
            base.Enter(time);
            _MainBaseVO._IsAllowMobilizeTroop = true;
            _MainBaseVO._IsAllowBuildBuilding = true;
            _MainBaseVO._IsAllowResetBuildingPosition = true;

            _NextTrainSoldierTime = time + _MainBaseVO._TrainInterval;
            _NextCreateGrainTime = time + _MainBaseVO._GrainOutputInterval;
            _NextCreatGoldTime = time + _MainBaseVO._GoldOutputInterval;
        }

        public override void Update(float time)
        {
            //生产士兵
            while (_NextTrainSoldierTime < time)
            {
                TrainSoldier();
                _NextTrainSoldierTime += _MainBaseVO._TrainInterval;
            }
            //生产粮食
            while (_NextCreateGrainTime < time)
            {
                CreateGrain();
                _NextCreateGrainTime += _MainBaseVO._GrainOutputInterval;
            }
            //生产金矿
            while (_NextCreatGoldTime < time)
            {
                CreateGold();
                _NextCreatGoldTime += _MainBaseVO._GoldOutputInterval;
            }
        }

        private void CreateGold()
        {
            if (_MainBaseVO.ower.isGoldBelowLimit)
            {
                int temp = _MainBaseVO.ower.goldLimit - _MainBaseVO.ower.gold;
                temp = Math.Min((int)_MainBaseVO.goldOutputNum, temp);
                _MainBaseVO.ower.gold += temp;
            }
        }

        private void CreateGrain()
        {
            if (_MainBaseVO.ower.isGrainBelowLimit)
            {
                int temp = _MainBaseVO.ower.grainLimit - _MainBaseVO.ower.grain;
                temp = Math.Min((int)_MainBaseVO.grainOutputNum, temp);
                _MainBaseVO.ower.grain += temp;
            }
        }

        private void TrainSoldier()
        {
            if (_MainBaseVO.isSoldierBelowLimit && _MainBaseVO.ower.isSoldierBelowLimit)
            {
                int temp = _MainBaseVO.ower.soldierAmountLimit - _MainBaseVO.ower.soldierAmount;
                temp = Math.Min(temp, _MainBaseVO.soldierNumLimit - _MainBaseVO.soldierNum);
                temp = Math.Min(temp, (int)_MainBaseVO._TrainNum);
                _MainBaseVO.soldierNum += temp;
                _MainBaseVO.ower.soldierAmount += temp;
            }
        }

        public override void ReceiveSoldier(PlayerVO player, float time)
        {
            base.ReceiveSoldier(player, time);

            ChangePlayer(player);
        }
    }

    private class OccupyingMainBaseState : MainBaseState
    {
        private bool _IsReset = false;
        private float _OccupiedTime = 0;
        private PlayerVO _Player;
        public OccupyingMainBaseState()
        {
            _EMainBaseState = EMainBaseState.Occupying;
        }
        public override void Enter(float time)
        {
            base.Enter(time);
            _MainBaseVO._IsAllowMobilizeTroop = false;
            _MainBaseVO._IsAllowBuildBuilding = false;
            _MainBaseVO._IsAllowResetBuildingPosition = false;
            _OccupiedTime = time + GlobalSetting.BULDING_MAINBASE_OCCUPIED_TIME;

        }

        public override void Update(float time)
        {
            if (time > _OccupiedTime)
            {
                //TODO 处理占领逻辑
            }
        }

        public override void ReceiveSoldier(PlayerVO player, float time)
        {
            base.ReceiveSoldier(player, time);

            if (!player.Id.Equals(_OccuptedPlayerID))
            {
                _OccupiedTime = time + GlobalSetting.BULDING_MAINBASE_OCCUPIED_TIME;
                _Player = player;
            }

            ChangePlayer(player);
        }
    }
}