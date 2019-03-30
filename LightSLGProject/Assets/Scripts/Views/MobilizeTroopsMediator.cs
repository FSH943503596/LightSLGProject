/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-12-16:53:29
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

public class MobilizeTroopsMediator : Mediator
{
    new public const string NAME = "MobilizeTroopsMediator";

    public MobilizeTroopsMediator() : base(NAME) { }

    private MainBaseVO _OriginMainBaseVO = null;
    private MainBaseVO _TargetMainBaseVO = null;

    protected MobilizeTroopsInfoUIForm _UIForm => base.m_viewComponent as MobilizeTroopsInfoUIForm;

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_PickMainBase,
            GlobalSetting.Msg_InitMobilizeTroopsMediator,
            GlobalSetting.Msg_EndBattle     
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitMobilizeTroopsMediator:
                MobilizeTroopsInfoUIForm uiform = notification.Body as MobilizeTroopsInfoUIForm;
                if (uiform)
                {
                    InitMobilizeTroopsMediator(uiform);
                }
                break;
            case GlobalSetting.Msg_PickMainBase:
                MainBaseVO vO = notification.Body as MainBaseVO;
                if (vO != null)
                {
                    if (_OriginMainBaseVO == null && _TargetMainBaseVO == null)
                    {
                        _UIForm.OpenUIForm(GlobalSetting.UI_MobilizeTroopsInfoUIForm);
                    }

                    

                    if (vO.ower.IsUser && _OriginMainBaseVO == null)
                    {
                        _OriginMainBaseVO = vO;
                        _UIForm.SetOriginInfo(vO.tilePositon, vO.soldierNum / 2);
                    }
                    else if ( _OriginMainBaseVO != null && vO.Equals(_OriginMainBaseVO))
                    {
                        return;
                    }
                    else
                    {
                        
                        _TargetMainBaseVO = vO;
                        _UIForm.SetTargetInfo(vO.tilePositon);
                    }

                    if (_OriginMainBaseVO != null && _TargetMainBaseVO != null)
                    {
                        var threeCmdParams = TreeMsgParamsPool<MainBaseVO, MainBaseVO, int>.Instance.Pop();
                        threeCmdParams.InitParams(_OriginMainBaseVO, _TargetMainBaseVO, _OriginMainBaseVO.soldierNum / 2);
                        SendNotification(GlobalSetting.Cmd_MoveTroops, threeCmdParams);

                        _OriginMainBaseVO = null;
                        _TargetMainBaseVO = null;
                        _UIForm.DelayClose(1);
                    }
                }
                break;
            case GlobalSetting.Msg_EndBattle:
                _OriginMainBaseVO = null;
                _TargetMainBaseVO = null;
                _UIForm.CloseUIForm();
                break;
            default:
                break;
        }
    }

    private void InitMobilizeTroopsMediator(MobilizeTroopsInfoUIForm uiform)
    {
        base.m_viewComponent = uiform;

        _UIForm.BtnOrigin.onClick.AddListener(()=>MoveCameraToMainBase(_OriginMainBaseVO));
        _UIForm.BtnTarget.onClick.AddListener(()=> MoveCameraToMainBase(_TargetMainBaseVO));
        _UIForm.BtnCancel.onClick.AddListener(MoveTroopsCancel);
    }

    private void MoveTroopsCancel()
    {
        _OriginMainBaseVO = null;
        _TargetMainBaseVO = null;
        _UIForm.CloseUIForm();
    }

    private void MoveCameraToMainBase(MainBaseVO mainBaseVO)
    {
        var positon = TwoMsgParamsPool<float, float>.Instance.Pop();
        positon.InitParams(mainBaseVO.postion.x, mainBaseVO.postion.z);
        SendNotification(GlobalSetting.Msg_CameraFocusPoint, positon);
    }
}

