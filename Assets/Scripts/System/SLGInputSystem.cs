using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;
using System;

public class SLGInputSystem :IBattleSystem<BattleManager>
{
    public SLGInputSystem(IBattleManager mgr) : base(mgr) { }

    private Transform _CameraRoot;
    private Camera _Camera;
    private bool _IsAcceptCommand = false;
    private Transform _LongTapItem;
    private Transform _DragItem;
    private ConstructionMediator _ConstructionMediator;
    private MapVOProxy _MapProxy;
    private Vector3Int _LastDragPosition;
    private int _GroundLayerMask;
    private Action<float> _ChangeViewRect;

    public Transform longTapItem { get => _LongTapItem; set => _LongTapItem = value; }
    public Transform dragItem { get => _DragItem; set => _DragItem = value; }

    public override void Initialize()
    {
        _Camera = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_BATTLE_SCENE_CAMERA_NAME).GetComponent<Camera>();
        _CameraRoot = _Camera.transform.parent;
        if (_Camera.orthographic) _ChangeViewRect = OrthographicCameraChangeViewRect;
        else _ChangeViewRect = PerspectiveCameraChangeViewRect;


        _ConstructionMediator = facade.RetrieveMediator(ConstructionMediator.NAME) as ConstructionMediator;
        _MapProxy = facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;

        _GroundLayerMask = LayerMask.GetMask(GlobalSetting.LAYER_MASK_NAME_GROUND);

        _IsAcceptCommand = true;
    }

    public override void Update ()
    {

        if (!_IsAcceptCommand) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var paramater = OneMsgParamsPool<float>.Instance.Pop();
            paramater.InitParams(30 * Time.deltaTime);
            facade.SendNotification(GlobalSetting.Msg_AdjustFocuses, paramater) ;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var paramater = OneMsgParamsPool<float>.Instance.Pop();
            paramater.InitParams(-30 * Time.deltaTime);
            facade.SendNotification(GlobalSetting.Msg_AdjustFocuses, paramater);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //如果点击到我方区域内
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //如果点下是点击到我方区域，抬起是点击到另一个占领区域

            //如果没有点击到区域，右键按下的信息
        }
#endif

        Gesture current = EasyTouch.current;

        if (current == null) return;

        if (current.type == EasyTouch.EvtType.On_SimpleTap)
        {
            //如果点击到空白处，显示切换建造界面，否则显示信息   
            if (current.pickedObject == null && !_DragItem && !_ConstructionMediator.IsShowBuildingList)
            {
                _LongTapItem = null;
                _ConstructionMediator.ShowBuildingSelectList();
            }
            else if(current.pickedObject != null && !_DragItem)
            {//如果点击到交互物体
                _ConstructionMediator.PickBuilding(current.pickedObject);
            }
        }

        if (current.type == EasyTouch.EvtType.On_LongTapStart && current.pickedObject != null)
        {
            _LongTapItem = current.pickedObject.transform.parent;
            
            if (_LongTapItem != null)
            {                
                _ConstructionMediator.SelectBuilding(_LongTapItem);
            }         
        }

        if (current.type == EasyTouch.EvtType.On_DragStart && current.pickedObject != null)
        {
            if (_LongTapItem == current.pickedObject.transform.parent)
            {
                _DragItem = current.pickedObject.transform.parent;
                SaveLastPosition(current);
            }
        }

        if (current.type == EasyTouch.EvtType.On_Drag && current.pickedObject != null)
        {
            if (_DragItem == current.pickedObject.transform.parent)
            {
 
                Vector3 pos = GetCurrentPointInMap(current.position);

                if (pos == default) return;

                if (Vector3.SqrMagnitude(_LastDragPosition - pos) > 0.8)
                {
                    pos = _DragItem.parent.worldToLocalMatrix * pos;
                    Vector3Int temp = new Vector3Int((int)pos.x, 0, (int)pos.z);
                    _DragItem.localPosition = temp + Vector3.up * 0.1f;
                    _ConstructionMediator.MoveBuilding(temp);
                    SaveLastPosition(current);
                }
            }
        }

        if (current.type == EasyTouch.EvtType.On_Swipe && current.touchCount == 1)
        {
            //cameraRoot.Translate(Vector3.left * current.deltaPosition.x / Screen.width * 20);
            //cameraRoot.Translate(Vector3.back * current.deltaPosition.y / Screen.height * 20);
            var moveParamer = TwoMsgParamsPool<float, float>.Instance.Pop();
            moveParamer.InitParams(current.deltaPosition.x / Screen.width * 20, current.deltaPosition.y / Screen.height * 20);
            facade.SendNotification(GlobalSetting.Msg_MoveCamera, moveParamer);
        }


        if (current.type == EasyTouch.EvtType.On_Pinch)
        {
            
            _ChangeViewRect(current.deltaPinch * 10 * Time.deltaTime);
        }
    }

    public override void Release()
    {
        base.Release();
        _IsAcceptCommand = false;
        _CameraRoot = null;
        _Camera = null;
        _LongTapItem = null;
        _DragItem = null;
        _LastDragPosition = default;
        _ChangeViewRect = null;
    }

    private void SaveLastPosition(Gesture current)
    {
        Vector3 vector3 = GetCurrentPointInMap(current.position);
        vector3 = _DragItem.worldToLocalMatrix * vector3;
        _LastDragPosition = new Vector3Int((int)vector3.x, 0, (int)vector3.z);
    }

    private Vector3 GetCurrentPointInMap(Vector2 screenPosition)
    {
        Vector3 result = default;

        Ray ray = _Camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, _GroundLayerMask))
        {
            result =  new Vector3(hitInfo.point.x,0, hitInfo.point.z);
        }

        return result;
    }

    private void PerspectiveCameraChangeViewRect(float vo)
    {
        _Camera.fieldOfView += vo;
    }

    private void OrthographicCameraChangeViewRect(float vo)
    {
        var paramater = OneMsgParamsPool<float>.Instance.Pop();
        paramater.InitParams(30 * Time.deltaTime);
        facade.SendNotification(GlobalSetting.Msg_AdjustFocuses, paramater);
    }
}