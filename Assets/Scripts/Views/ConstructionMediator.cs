/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-27-16:36:53
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using SUIFW;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionMediator : Mediator
{
    new public static string NAME = "ConstructionMediator";

    private string ConstructionHUD1Name = "HUD_ConstructionOne";
    private string ConstructionHUD4Name = "HUD_ConstructionFour";

    private bool _IsShowBuildingList = true;

    private Transform _BuildingParnet;
    private Transform _HUDParent;
    private MapVOProxy _MapProxy;
    private PlayerVOProxy _UserProxy;
    private BuildingVOProxy _BuildingProxy;
    private IBuildingVO _Building;
    private Transform _BuildingTF;
    private HUDConstruction _CurrentIndicator;
    private Func<bool> _IsCanConfirmFunc;
    private bool isCreating = false;
    private PlayerVO _UserPlayerVO;

    private Dictionary<GameObject, IBuildingVO> _GameObjectToVO = new Dictionary<GameObject, IBuildingVO>();
    private Dictionary<GameObject, MainBaseVO> _GameOjectToMainbaseVO = new Dictionary<GameObject, MainBaseVO>();
    private Dictionary<MainBaseVO, MainBase> _MainBaseVOToMainBase = new Dictionary<MainBaseVO, MainBase>();
    private Dictionary<GameObject, MainBaseVO> _UpLevelGOToMainbaseVO = new Dictionary<GameObject, MainBaseVO>();

    private ConstructionUIForm uiForm
    {
        get {
            return base.ViewComponent as ConstructionUIForm;
        }
    }

    public bool IsShowBuildingList { get => _IsShowBuildingList; }

    public ConstructionMediator() : base(NAME) { }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            GlobalSetting.Msg_InitConstructionMediator,
            GlobalSetting.Msg_BuildBuilding,
            GlobalSetting.Msg_ChangeMainBaseLevelUpState,
            GlobalSetting.Msg_UpdateMainBase
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case GlobalSetting.Msg_ChangeMainBaseLevelUpState:
                var mainbaseLevelUpState = notification.Body as Dictionary<MainBaseVO, bool>;
                if (mainbaseLevelUpState != null)
                {
                    ChangeMainbaseLevelUpState(mainbaseLevelUpState);
                }
                break;
            case GlobalSetting.Msg_InitConstructionMediator:
                ConstructionUIForm constructionUIForm = notification.Body as ConstructionUIForm;
                if (constructionUIForm)
                {
                    InitConstructionMediator(constructionUIForm);
                }
                break;
            case GlobalSetting.Msg_BuildBuilding:
                IBuildingVO buildingVO = notification.Body as IBuildingVO;
                if (buildingVO != null)
                {
                    BuildBuilding(buildingVO);
                }
                break;
            case GlobalSetting.Msg_UpdateMainBase:
                var mainBase = notification.Body as MainBaseVO;
                if (mainBase != null && _MainBaseVOToMainBase.ContainsKey(mainBase))
                {
                    _MainBaseVOToMainBase[mainBase].UpdateArea();
                }
                break;
            default:
                break;
        }
    }

    private void ChangeMainbaseLevelUpState(Dictionary<MainBaseVO, bool> mainbaseLevelUpState)
    {
        PoolManager.Instance.HideAllObject("HUD_MainBaseUpLevel");
        var enumberator = mainbaseLevelUpState.GetEnumerator();
        Transform hudTF;
        while (enumberator.MoveNext())
        {
            if (enumberator.Current.Value)
            {
                hudTF = PoolManager.Instance.GetObject("HUD_MainBaseUpLevel", LoadAssetType.Normal, _HUDParent).transform;
                hudTF.position = enumberator.Current.Key.postion;
                if (_UpLevelGOToMainbaseVO.ContainsKey(hudTF.gameObject))
                {
                    _UpLevelGOToMainbaseVO[hudTF.gameObject] = enumberator.Current.Key;
                }
                else
                {
                    _UpLevelGOToMainbaseVO.Add(hudTF.gameObject, enumberator.Current.Key);
                }
            }
        }
    }

    public void InitConstructionMediator(ConstructionUIForm baseUIForm)
    {
        base.ViewComponent = baseUIForm;

        ConstructionUIForm form = uiForm;

        uiForm.btnMainBase.onClick.AddListener(() => CreateBuilding(E_Building.MainBase));
        uiForm.btnMilitaryCamp.onClick.AddListener(() => CreateBuilding(E_Building.MilitaryCamp));
        uiForm.btnFarmLand.onClick.AddListener(() => CreateBuilding(E_Building.FarmLand));
        uiForm.btnGoldMine.onClick.AddListener(() => CreateBuilding(E_Building.GoldMine));

        uiForm.btnCancel.onClick.AddListener(Cancel);
        uiForm.btnConfirm.onClick.AddListener(Confirm);
        uiForm.btnInfo.onClick.AddListener(ShowBuildingInfo);
        uiForm.btnTurn.onClick.AddListener(RotateBuilding);

        _BuildingParnet = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_BUILDING_PARENT_NAME).transform;
        _HUDParent = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_HUD_PARENT_NAME).transform;
        _MapProxy = Facade.RetrieveProxy(MapVOProxy.NAME) as MapVOProxy;

        _UserProxy = Facade.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy;

        _BuildingProxy = Facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;

        CreateBuildingIndicator();
    }

    private void CreateBuildingIndicator()
    {
        PoolManager.Instance.IncreaseObjectCache(ConstructionHUD1Name, 1);
        PoolManager.Instance.IncreaseObjectCache(ConstructionHUD4Name, 1);    
    }

    private void Confirm()
    {
        //判断是否能够放置
        if (_IsCanConfirmFunc())
        {
            //取消指示器
            _CurrentIndicator.transform.SetParent(null);
            PoolManager.Instance.HideObjet(_CurrentIndicator.gameObject);
            //隐藏展示建筑
            PoolManager.Instance.HideObjet(_BuildingTF.gameObject);
            //显示建造列表
            ShowBuildingSelectList();
            //发送创建建筑消息 增加建筑信息，取消选中状态
            SendNotification(GlobalSetting.Cmd_ConfirmConstruction, _Building);
        }
    }

    private void Cancel()
    {
        //取消当前显示内容
        _CurrentIndicator.transform.SetParent(null);
        PoolManager.Instance.HideObjet(_CurrentIndicator.gameObject);
        PoolManager.Instance.HideObjet(_BuildingTF.gameObject);

        //清空建造数据
        _Building = null;
        _BuildingTF = null;
        _CurrentIndicator = null;
        //变更显示界面
        ShowBuildingSelectList();
        //取消选中拖拽操作
        SendNotification(GlobalSetting.Cmd_CancelConstruction);
    }

    private void RotateBuilding()
    {
        _Building.Rotate();

        _BuildingTF.localRotation = Quaternion.Euler(0, _Building.rotateValue, 0);

        _CurrentIndicator.SetIsCanConfirm(_IsCanConfirmFunc());
    }

    private void ShowBuildingInfo()
    {
        
    }

    private void CreateBuilding(E_Building mainBase)
    {
        string prefabName = "";
        string indicatorName = "";
        _CurrentIndicator = null;
        _BuildingTF = null;
        _Building = null;
        switch (mainBase)
        {
            case E_Building.None:
                break;
            case E_Building.MainBase:
                prefabName = "MainBase";
                indicatorName = ConstructionHUD1Name;
                MainBaseVO vo = new MainBaseVO();
                vo.isMain = false;
                vo.prefabName = MainBaseVO.SubPrefabName;
                _Building = vo;
                _IsCanConfirmFunc = () => IsCanBuildMainBase(vo);
                break;
            case E_Building.FarmLand:
                prefabName = "FarmLand";
                indicatorName = ConstructionHUD4Name;
                _Building = new FarmLandVO();
                _IsCanConfirmFunc = () => _UserProxy.IsCanConstructionUserBuilding(0, _Building);
                break;
            case E_Building.GoldMine:
                prefabName = "GoldMine";
                indicatorName = ConstructionHUD4Name;
                _Building = new GoldMineVO();
                _IsCanConfirmFunc = () => _UserProxy.IsCanConstructionUserBuilding(0, _Building);
                break;
            case E_Building.MilitaryCamp:
                prefabName = "MilitaryCamp";
                indicatorName = ConstructionHUD4Name;
                _Building = new MilitaryCampVO();
                _IsCanConfirmFunc = () => _UserProxy.IsCanConstructionUserBuilding(0, _Building);
                break;
            default:
                break;
        }

        _CurrentIndicator = PoolManager.Instance.GetObject(indicatorName, LoadAssetType.Normal, null).GetComponent<HUDConstruction>();

        if (_CurrentIndicator == null || string.IsNullOrEmpty(prefabName)) return;

        _BuildingTF = PoolManager.Instance.GetObject(prefabName, LoadAssetType.Normal, null).transform;
        _CurrentIndicator.transform.SetParent(_BuildingTF);
        _CurrentIndicator.transform.localPosition = Vector3.zero;
        _CurrentIndicator.transform.localRotation = Quaternion.identity;

        _BuildingTF.SetParent(_BuildingParnet);
        _BuildingTF.localRotation = Quaternion.identity;
        _Building.tilePositon = _MapProxy.ViewPositionToMap(new Vector3(0.5f, 0.5f, 0));
        _BuildingTF.localPosition = _Building.tilePositon + Vector3.up * 0.58f;
        SendNotification(GlobalSetting.Cmd_ShowConstruction, _BuildingTF);

        this.isCreating = true;
        ShowBuildingSetup();

        _CurrentIndicator.SetIsCanConfirm(_IsCanConfirmFunc());
    }

    public void ShowBuildingSelectList()
    {
        uiForm.ShowBuildingSelectList();
        _IsShowBuildingList = true;
    }

    public void ShowBuildingSetup()
    {
        uiForm.ShowBuildingSetup(isCreating);
        _IsShowBuildingList = false;


    }

    /// <summary>
    /// 长按选择一个建筑
    /// </summary>
    /// <param name="selectedTF"></param>
    public void SelectBuilding(Transform selectedTF)
    {
        this.isCreating = false;
        ShowBuildingSetup();
    }

    /// <summary>
    /// 单击选中一个建筑
    /// </summary>
    /// <param name="selectedTF"></param>
    public void PickBuilding(GameObject selectedTF)
    {
        if (selectedTF && selectedTF.transform.parent)
        {
            if (_UpLevelGOToMainbaseVO.ContainsKey(selectedTF.transform.parent.gameObject))
            {
                SendNotification(GlobalSetting.Cmd_UpdateMainBase, _UpLevelGOToMainbaseVO[selectedTF.transform.parent.gameObject]);
            }
            else if (_GameObjectToVO.ContainsKey(selectedTF.transform.parent.gameObject))
            {
                IBuildingVO pickItem = _GameObjectToVO[selectedTF.transform.parent.gameObject];
                if (pickItem.buildingType == E_Building.MainBase)
                {
                    SendNotification(GlobalSetting.Cmd_PickMainBase, pickItem);
                }
            } 
        }
    }

    /// <summary>
    /// 移动建筑
    /// </summary>
    /// <param name="mapPosition"></param>
    public void MoveBuilding(Vector3Int mapPosition)
    {
        _Building.tilePositon = mapPosition;

        _CurrentIndicator.SetIsCanConfirm(_IsCanConfirmFunc());
    }

    private bool IsCanBuildMainBase(MainBaseVO vo)
    {
        bool isCanBuild = _MapProxy.IsCanOccupedArea(vo.tilePositon, vo.radius);
        if (isCanBuild) _BuildingProxy.VisitMainBases(mb => isCanBuild = isCanBuild && GetMainBaseLength(mb, vo) > GlobalSetting.BUILDING_MAINBASE_BUILD_MIN_LENGTH);
        return isCanBuild;
    }

    private ushort GetMainBaseLength(MainBaseVO mb, MainBaseVO vo)
    {
        return (ushort)(Math.Abs(mb.tilePositon.x - vo.tilePositon.x) + Math.Abs(mb.tilePositon.z - vo.tilePositon.z));
    }

    private void BuildBuilding(IBuildingVO vO)
    {
        _BuildingTF = PoolManager.Instance.GetObject(vO.prefabName, LoadAssetType.Normal, null).transform;
        _BuildingTF.SetParent(_BuildingParnet);
        _BuildingTF.localRotation = Quaternion.Euler(0, vO.rotateValue, 0);
        _BuildingTF.localPosition = vO.tilePositon;

        _GameObjectToVO.Add(_BuildingTF.gameObject, vO);
        if (vO.buildingType == E_Building.MainBase)
        {
            _GameOjectToMainbaseVO.Add(_BuildingTF.gameObject, vO as MainBaseVO);
            var mainbase = _BuildingTF.gameObject.GetComponent<MainBase>();
            _MainBaseVOToMainBase.Add(vO as MainBaseVO, mainbase);
            mainbase.InitMainBase(vO as MainBaseVO);
        }

        vO.postion = _BuildingTF.position;
    }
}

