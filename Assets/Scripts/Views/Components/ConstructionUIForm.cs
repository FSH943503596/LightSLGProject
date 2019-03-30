/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-27-15:07:31
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;
using UnityEngine.UI;
using SUIFW;
public class ConstructionUIForm :BaseUIForm
{
    [SerializeField] private Transform _BuildingSelectList;
    [SerializeField] private Button _BtnMainBase;
    [SerializeField] private Button _BtnMilitaryCamp;
    [SerializeField] private Button _BtnGoldMine;
    [SerializeField] private Button _BtnFarmLand;

    [SerializeField] private Text _TxtCreateSubBaseGoldCost;
    [SerializeField] private Text _TxtCreateSubBaseGrainCost;
    [SerializeField] private Text _TxtCreateMilitaryGoldCost;
    [SerializeField] private Text _TxtCreateMilitaryGrainCost;
    [SerializeField] private Text _TxtCreateGoldMineGoldCost;
    [SerializeField] private Text _TxtCreateGoldMineGrainCost;
    [SerializeField] private Text _TxtCreateFarmLandGoldCost;
    [SerializeField] private Text _TxtCreateFarmLandGrainCost;

    [SerializeField] private Transform _BuildingSetup;
    [SerializeField] private Button _BtnCancel;
    [SerializeField] private Button _BtnInfo;
    [SerializeField] private Button _BtnTurn;
    [SerializeField] private Button _BtnConfirm;

    public Button btnMainBase => _BtnMainBase;
    public Button btnMilitaryCamp => _BtnMilitaryCamp;
    public Button btnGoldMine => _BtnGoldMine;
    public Button btnFarmLand => _BtnFarmLand;
    public Button btnCancel => _BtnCancel;
    public Button btnInfo => _BtnInfo;
    public Button btnTurn => _BtnTurn;
    public Button btnConfirm => _BtnConfirm;
    public Text txtCreateSubBaseGoldCost => _TxtCreateSubBaseGoldCost;
    public Text txtCreateSubBaseGrainCost => _TxtCreateSubBaseGrainCost;
    public Text txtCreateMilitaryGoldCost => _TxtCreateMilitaryGoldCost; 
    public Text txtCreateMilitaryGrainCost => _TxtCreateMilitaryGrainCost;
    public Text txtCreateGoldMineGoldCost => _TxtCreateGoldMineGoldCost;
    public Text txtCreateGoldMineGrainCost => _TxtCreateGoldMineGrainCost;
    public Text txtCreateFarmLandGoldCost => _TxtCreateFarmLandGoldCost;
    public Text txtCreateFarmLandGrainCost => _TxtCreateFarmLandGrainCost;

    public void ShowBuildingSelectList()
    {
        _BuildingSelectList.gameObject.SetActive(true);
        _BuildingSetup.gameObject.SetActive(false); 
    }

    public void ShowBuildingSetup(bool isCreate = true)
    {
        _BuildingSelectList.gameObject.SetActive(false);
        _BuildingSetup.gameObject.SetActive(true);

        _BtnInfo.gameObject.SetActive(!isCreate);
        _BtnCancel.gameObject.SetActive(isCreate);
    }

    public void SetConfirmState(bool isCanConfirm)
    {
        btnConfirm.interactable = isCanConfirm;
    }
}
