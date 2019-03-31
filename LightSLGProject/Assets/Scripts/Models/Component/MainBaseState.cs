/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-27-11:09:17
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public abstract class MainBaseState
{
    protected MainBaseVO _MainBaseVO;
    protected float startTime = 0;
    protected EMainBaseState _EMainBaseState = EMainBaseState.None;
    protected PlayerVO _OccuptedPlayer;
    protected Action<string, object> _SendMsgAction;

    public MainBaseState(Action<string, object> sendMsgAction)
    {
        _SendMsgAction = sendMsgAction;
    }

    public abstract bool IsNeedChange(float time);
    public EMainBaseState eMainBaseState => _EMainBaseState;
    public PlayerVO occuptedPlayer { get => _OccuptedPlayer; set => _OccuptedPlayer = value; }
    public MainBaseVO mainBaseVO { get => _MainBaseVO; set => _MainBaseVO = value; }
    public virtual void Init(MainBaseVO mainBaseVO, PlayerVO occupiedPlayer)
    {
        _MainBaseVO = mainBaseVO;
        _OccuptedPlayer = occupiedPlayer;
        var messageParam = TwoMsgParamsPool<PlayerVO, MainBaseVO>.Instance.Pop();
        messageParam.InitParams(_MainBaseVO.ower, mainBaseVO);
        if (_MainBaseVO.ower != _OccuptedPlayer)
        {       
            _MainBaseVO.SetOwer(occupiedPlayer);
        }
        _SendMsgAction(GlobalSetting.Cmd_MainBaseChangeOwer, messageParam);

    }
    public virtual void Enter(float time)
    {
        startTime = time;
    }
    public virtual void Update(float time) { }
    public virtual void ReceiveSoldier(PlayerVO player, float time)
    {
        if (_OccuptedPlayer != player)
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
            _OccuptedPlayer = player;
            _MainBaseVO.soldierNum *= -1;
        }
    }
}

