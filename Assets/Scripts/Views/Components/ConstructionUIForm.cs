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

    [SerializeField] private Transform _BuildingSetup;
    [SerializeField] private Button _BtnCancel;
    [SerializeField] private Button _BtnInfo;
    [SerializeField] private Button _BtnTurn;
    [SerializeField] private Button _BtnConfirm;

    public Button btnMainBase { get => _BtnMainBase;}
    public Button btnMilitaryCamp { get => _BtnMilitaryCamp;}
    public Button btnGoldMine { get => _BtnGoldMine;}
    public Button btnFarmLand { get => _BtnFarmLand;}
    public Button btnCancel { get => _BtnCancel;}
    public Button btnInfo { get => _BtnInfo;}
    public Button btnTurn { get => _BtnTurn;}
    public Button btnConfirm { get => _BtnConfirm;}

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
