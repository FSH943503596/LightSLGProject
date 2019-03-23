/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-20-11:47:33
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleInfoMediator : Mediator
{
    new public const string NAME = "PlayerBattleInfoMediator";

    public PlayerBattleInfoMediator() : base(NAME) { }

    private bool _UsersPlayerVODirty = false;
    private PlayerVO _UsersPlayerVO = null;
    private PlayerBattleInfoUIForm _UIForm => m_viewComponent as PlayerBattleInfoUIForm;

    public void InitPlayerBattleInfoMediator(PlayerBattleInfoUIForm uiForm)
    {
        m_viewComponent = uiForm;
        uiForm.preRanderAction = UpdateBaseUI;
        uiForm.SetDefaultInfo();
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitPlayerBattleInfoMediator,
            GlobalSetting.Msg_UsersPlayerCreated,
            GlobalSetting.Msg_SetUsersPlayerBattleInfoDirty
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_SetUsersPlayerBattleInfoDirty:
                _UsersPlayerVODirty = true;
                break;
            case GlobalSetting.Msg_InitPlayerBattleInfoMediator:
                var uiform = notification.Body as PlayerBattleInfoUIForm;
                if (uiform)
                {
                    InitPlayerBattleInfoMediator(uiform);
                }
                break;
            case GlobalSetting.Msg_UsersPlayerCreated:
                var player = notification.Body as PlayerVO;
                if (player != null)
                {
                    InitUsersPlayerInfo(player);
                }
                break;
            default:
                break;
        }
    }

    private void InitUsersPlayerInfo(PlayerVO vo)
    {
        _UsersPlayerVO = vo;

        _UIForm.SetPlayerFlag(vo.colorIndex);
    }

    private void UpdatePlayerInfo()
    {
        _UIForm.SetMainBaseCount(_UsersPlayerVO.mainBases.Count);
        _UIForm.SetSoldierCount(_UsersPlayerVO.soldierAmount, _UsersPlayerVO.soldierAmountLimit);
        _UIForm.SetGoldInfo(_UsersPlayerVO.gold, _UsersPlayerVO.goldLimit);
        _UIForm.SetGrainInfo(_UsersPlayerVO.grain, _UsersPlayerVO.grainLimit);
    }

    private void UpdateBaseUI()
    {
        if (_UsersPlayerVODirty)
        {
            UpdatePlayerInfo();
            _UsersPlayerVODirty = false;
        }
    }
}

