/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-25-13:41:12
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;

public class BattleCameraController : MonoBehaviour
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


    private void Start()
    {
        _Camera = Camera.main;
        _CameraTF = _Camera.transform;
        _CameraParentTF = _Camera.transform.parent;



        _LineParamA = (_MaxAngle - _MinAngle) / (_MaxR - _MinR);
        _LineParamB = _MinAngle - _LineParamA * _MinR;

        _CameraParentTF.position = new Vector3(_OriginCenter.x, _Hight, _OriginCenter.z);
        _Angle = 90;
        _CameraTF.localRotation = Quaternion.Euler(90, 0, 0);
        _R = _MaxR;

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
