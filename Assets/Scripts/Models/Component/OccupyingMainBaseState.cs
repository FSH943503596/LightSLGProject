/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-27-11:48:35
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class OccupyingMainBaseState : MainBaseState
{
    private bool _IsReset = false;
    private float _OccupiedTime = 0;
    private PlayerVO _Player;

    public override bool IsNeedChange(float time)
    {
        return _OccupiedTime < time;
    }

    public OccupyingMainBaseState(Action<string, object> sendMsgAction) : base(sendMsgAction)
    {
        _EMainBaseState = EMainBaseState.Occupying;
    }
    public override void Enter(float time)
    {
        base.Enter(time);
        _MainBaseVO.isAllowMobilizeTroop = false;
        _MainBaseVO.isAllowBuildBuilding = false;
        _MainBaseVO.isAllowResetBuildingPosition = false;
        _OccupiedTime = time + GlobalSetting.BULDING_MAINBASE_OCCUPIED_TIME;

    }

    public override void Update(float time)
    {

    }

    public override void ReceiveSoldier(PlayerVO player, float time)
    {
        base.ReceiveSoldier(player, time);

        if (!player.Equals(_OccuptedPlayer))
        {
            _OccupiedTime = time + GlobalSetting.BULDING_MAINBASE_OCCUPIED_TIME;
            _Player = player;
        }

        ChangePlayer(player);
    }
}
