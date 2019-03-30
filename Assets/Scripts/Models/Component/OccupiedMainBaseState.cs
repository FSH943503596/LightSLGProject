/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-27-11:10:01
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class OccupiedMainBaseState : MainBaseState
{
    private float _NextTrainSoldierTime = 0;
    private float _NextCreateGrainTime = 0;
    private float _NextCreatGoldTime = 0;

    public override bool IsNeedChange(float time)
    {
        return !_MainBaseVO.ower.Equals(_OccuptedPlayer);
    }

    public OccupiedMainBaseState(Action<string, object> sendMsgAction):base(sendMsgAction)
    {
        _EMainBaseState = EMainBaseState.Occupied;
    }

    public override void Enter(float time)
    {
        base.Enter(time);
        _MainBaseVO.isAllowMobilizeTroop = true;
        _MainBaseVO.isAllowBuildBuilding = true;
        _MainBaseVO.isAllowResetBuildingPosition = true;

        _NextTrainSoldierTime = time + _MainBaseVO.trainInterval;
        _NextCreateGrainTime = time + _MainBaseVO.grainOutputInterval;
        _NextCreatGoldTime = time + _MainBaseVO.goldOutputInterval;
    }

    public override void Update(float time)
    {
        //生产士兵
        while (_NextTrainSoldierTime < time)
        {
            TrainSoldier();
            _NextTrainSoldierTime += _MainBaseVO.trainInterval;
        }
        //生产粮食
        while (_NextCreateGrainTime < time)
        {
            CreateGrain();
            _NextCreateGrainTime += _MainBaseVO.grainOutputInterval;
        }
        //生产金矿
        while (_NextCreatGoldTime < time)
        {
            CreateGold();
            _NextCreatGoldTime += _MainBaseVO.goldOutputInterval;
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
            temp = Math.Min(temp, (int)_MainBaseVO.trainNum);
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

