/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-25-18:20:34
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

public class BattleCameraMediator : Mediator
{
    private Camera _Camera;
    private Transform _CameraTF;
    private Transform _CameraParentTF;

    private float _MinR;
    private float _MaxR;
    private float _MinAngle;
    private float _MaxAngle;
    private float _Hight;

    private Vector3 _OriginCenter;

    private float _LineParamA;
    private float _LineParamB;
    private float _R;
    private float _Angle;

    new public const string NAME = "BattleCameraMediator";
    public BattleCameraMediator() : base(NAME) { }

    public void InitBattleCameraMediator()
    {
        GameObject cameraGO = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_BATTLE_SCENE_CAMERA_NAME);
        _Camera = cameraGO.GetComponent<Camera>();
        _CameraTF = cameraGO.transform;
        _CameraParentTF = _CameraTF.parent;

        _MinR = GlobalSetting.CAMERA_MIN_Z_OFFSET;
        _MaxR = GlobalSetting.CAMERA_MAX_Z_OFFSET;
        _MinAngle = GlobalSetting.CAMERA_MIN_ANGLE;
        _MaxAngle = GlobalSetting.CAMERA_MAX_ANGLE;
        _Hight = GlobalSetting.CAMERA_HIGHT;

        _LineParamA = (_MaxAngle - _MinAngle) / (_MaxR - _MinR);
        _LineParamB = _MinAngle - _LineParamA * _MinR;

        _CameraParentTF.position = new Vector3(0, _Hight, 0);
        _Angle = 90;
        _CameraTF.localRotation = Quaternion.Euler(90, 0, 0);
        _R = _MaxR;

        SetCamera();
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitBattleCameraMediator,
            GlobalSetting.Msg_AdjustFocuses,
            GlobalSetting.Msg_MoveCamera,
            GlobalSetting.Msg_CameraFocusPoint,
            GlobalSetting.Msg_EndBattle,
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_InitBattleCameraMediator:
                InitBattleCameraMediator();
                break;
            case GlobalSetting.Msg_AdjustFocuses:
                var adjuestFocusesParam = notification.Body as OneMsgParams<float>;
                if (adjuestFocusesParam != null)
                {
                    AdjustFocuses(adjuestFocusesParam.parameter);
                    OneMsgParamsPool<float>.Instance.Push(adjuestFocusesParam);
                    adjuestFocusesParam = null;
                }
                break;
            case GlobalSetting.Msg_MoveCamera:
                var moveCameraParam = notification.Body as TwoMsgParams<float,float>;
                if (moveCameraParam != null)
                {
                    MoveCamera(moveCameraParam.first, moveCameraParam.second);
                    moveCameraParam = null;
                }
                break;
            case GlobalSetting.Msg_CameraFocusPoint:
                var focusParam = notification.Body as TwoMsgParams<float, float>;
                if (focusParam != null)
                {
                    FocusPoint(focusParam.first, focusParam.second);
                    focusParam = null;
                }
                break;
            case GlobalSetting.Msg_EndBattle:
                _Camera = null;
                _CameraTF = null;
                _CameraParentTF = null;
                break;
            default:
                break;
        }
    }

    private void FocusPoint(float first, float second)
    {
        Vector3 target = new Vector3(first, _Hight, second - _Hight / Mathf.Tan(_Angle * Mathf.Deg2Rad));
       
        iTween.MoveTo(_CameraParentTF.gameObject, target, 1.5f);
    }

    private void MoveCamera(float first, float second)
    {
        _CameraParentTF.Translate(Vector3.left * first + Vector3.back * second);
    }

    private void AdjustFocuses(float parameter)
    {
        _R += parameter;
        _R = Mathf.Min(_R, _MaxR);
        _R = Mathf.Max(_R, _MinR);
        SetCamera();
    }

    private void SetCamera()
    {
        float newAngle = _LineParamA * _R + _LineParamB;
        _CameraParentTF.position = _CameraParentTF.position - Vector3.forward * _Hight * (Mathf.Tan((90 - newAngle) * Mathf.Deg2Rad) - Mathf.Tan((90 - _Angle) * Mathf.Deg2Rad));
        _Angle = newAngle;
        _CameraTF.localRotation = Quaternion.Euler(_Angle, 0, 0);
        _Camera.orthographicSize = _R * Mathf.Sin(_Angle * Mathf.Deg2Rad);
    }
}

