using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;
using System;

public class SLGInputSystem :IBattleSystem<BattleManager>
{
    public SLGInputSystem(IBattleManager mgr) : base(mgr) { }

    private Transform cameraRoot;
    private Camera camera;

    private Transform longTapItem;
    private Transform dragItem;

    private ConstructionMediator constructionMediator;
    private MapVOProxy mapProxy;

    private Vector3Int lastDragPosition;

    private int groundLayerMask;
    private Action<float> ChangeViewRect;

    public Transform LongTapItem { get => longTapItem; set => longTapItem = value; }
    public Transform DragItem { get => dragItem; set => dragItem = value; }

    public override void Initialize()
    {
        camera = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_BATTLE_SCENE_CAMERA_NAME).GetComponent<Camera>();
        cameraRoot = camera.transform.parent;
        if (camera.orthographic) ChangeViewRect = OrthographicCameraChangeViewRect;
        else ChangeViewRect = PerspectiveCameraChangeViewRect;


        constructionMediator = facade.RetrieveMediator(ConstructionMediator.NAME) as ConstructionMediator;
        mapProxy = facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;

        groundLayerMask = LayerMask.GetMask(GlobalSetting.LAYER_MASK_NAME_GROUND);
    }

    public override void Update () {

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeViewRect(300 * Time.deltaTime);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeViewRect(- 300 * Time.deltaTime);
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
            if (current.pickedObject == null && !dragItem && !constructionMediator.IsShowBuildingList)
            {
                longTapItem = null;
                constructionMediator.ShowBuildingSelectList();
            }
            else if(current.pickedObject != null && !dragItem)
            {//如果点击到交互物体
                constructionMediator.PickBuilding(current.pickedObject);
            }
        }

        if (current.type == EasyTouch.EvtType.On_LongTapStart && current.pickedObject != null)
        {
            longTapItem = current.pickedObject.transform.parent;
            
            if (longTapItem != null)
            {                
                constructionMediator.SelectBuilding(longTapItem);
            }         
        }

        if (current.type == EasyTouch.EvtType.On_DragStart && current.pickedObject != null)
        {
            if (longTapItem == current.pickedObject.transform.parent)
            {
                dragItem = current.pickedObject.transform.parent;
                SaveLastPosition(current);
            }
        }

        if (current.type == EasyTouch.EvtType.On_Drag && current.pickedObject != null)
        {
            if (dragItem == current.pickedObject.transform.parent)
            {
 
                Vector3 pos = GetCurrentPointInMap(current.position);

                if (pos == default) return;

                if (Vector3.SqrMagnitude(lastDragPosition - pos) > 0.8)
                {
                    pos = dragItem.parent.worldToLocalMatrix * pos;
                    Vector3Int temp = new Vector3Int((int)pos.x, 0, (int)pos.z);
                    dragItem.localPosition = temp + Vector3.up * 0.1f;
                    constructionMediator.MoveBuilding(temp);
                    SaveLastPosition(current);
                }
            }
        }

        if (current.type == EasyTouch.EvtType.On_Swipe && current.touchCount == 1)
        {
            cameraRoot.Translate(Vector3.left * current.deltaPosition.x / Screen.width * 20);
            cameraRoot.Translate(Vector3.back * current.deltaPosition.y / Screen.height * 20);
        }


        if (current.type == EasyTouch.EvtType.On_Pinch)
        {
            
            ChangeViewRect(current.deltaPinch * 10 * Time.deltaTime);
        }
    }

    private void SaveLastPosition(Gesture current)
    {
        Vector3 vector3 = GetCurrentPointInMap(current.position);
        vector3 = dragItem.worldToLocalMatrix * vector3;
        lastDragPosition = new Vector3Int((int)vector3.x, 0, (int)vector3.z);
    }

    private Vector3 GetCurrentPointInMap(Vector2 screenPosition)
    {
        Vector3 result = default;

        Ray ray = camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, groundLayerMask))
        {
            result =  new Vector3(hitInfo.point.x,0, hitInfo.point.z);
        }

        return result;
    }

    private void PerspectiveCameraChangeViewRect(float vo)
    {
        camera.fieldOfView += vo;
    }

    private void OrthographicCameraChangeViewRect(float vo)
    {
        camera.orthographicSize += vo;
    }
}