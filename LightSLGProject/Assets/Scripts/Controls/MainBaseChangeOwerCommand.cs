/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-28-18:30:14
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class MainBaseChangeOwerCommand : SimpleCommand
{
    bool _OthersDonotHaveSoldier = true;
    public override void Execute(INotification notification)
    {
        var parameter = notification.Body as TwoMsgParams<PlayerVO, MainBaseVO>;
        if (parameter != null)
        {
            PlayerVO playerVO = parameter.first;
            MainBaseVO mainBaseVO = parameter.second;
            if (parameter.first != null && parameter.second != null)
            {
                PlayerVOProxy playerProxy = Facade.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy;
                BuildingVOProxy buildingProxy = Facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;

                if (mainBaseVO.ower != playerVO)
                {//占领

                    if (mainBaseVO.ower.IsUser)
                    {
                        bool isWin = mainBaseVO.ower.mainBases.Count == buildingProxy.mainBaseTotalCount;
                        playerProxy.VisitAllPlayers((p) =>
                        {
                            if (!p.Equals(mainBaseVO.ower))
                            {
                                _OthersDonotHaveSoldier = _OthersDonotHaveSoldier && p.soldierAmount <= 0;
                            }
                        });
                        isWin = isWin && _OthersDonotHaveSoldier;
                        if (isWin)
                        {
                            BattleManager.Instance.SetBattleResult(true);
                        }

                    }
                    else
                    {

                    }
                }
                else
                {//收复

                }
            }

            TwoMsgParamsPool<PlayerVO, MainBaseVO>.Instance.Push(parameter);
            parameter = null;
        }   
    }
}

