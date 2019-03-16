/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-12-16:24:49
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class MobilizeTroopsInfoUIForm : BaseUIForm
{
    [SerializeField] private Button _BtnOrigin;
    [SerializeField] private Button _BtnCancel;
    [SerializeField] private Button _BtnTarget;
    [SerializeField] private Text _TxtOriginPostion;
    [SerializeField] private Text _TxtTargetPosition;
    [SerializeField] private Text _TxtSoldiersNum;

    private bool _IsSetOrigin = false;
    private bool _IsSetTarget = false;
    private string _PositionStringFormat = "({0}:{1})";
    private string _SoldierStringFormat = "兵：{0}";
    private bool _IsDelayClose = false;
    private float _CloseTime = 0;

    public Button BtnOrigin => _BtnOrigin;
    public Button BtnCancel => _BtnCancel;
    public Button BtnTarget => _BtnTarget;

    public override void Display()
    {
        base.Display();

        _IsSetOrigin = false;
        _IsSetTarget = false;

        _BtnOrigin.gameObject.SetActive(false);
        _BtnCancel.gameObject.SetActive(false);
        _BtnTarget.gameObject.SetActive(false);

        _TxtOriginPostion.gameObject.SetActive(false);
        _TxtSoldiersNum.gameObject.SetActive(false);
        _TxtTargetPosition.gameObject.SetActive(false);

        _IsDelayClose = false;
    }

    public void SetOriginInfo(Vector3Int pos, int soldierNum)
    {
        if (!_BtnOrigin.gameObject.activeSelf)
        {
            _BtnOrigin.gameObject.SetActive(true);

            _TxtSoldiersNum.gameObject.SetActive(true);

            _TxtOriginPostion.gameObject.SetActive(true);
        }
        _TxtOriginPostion.text = string.Format(_PositionStringFormat, pos.x, pos.z);
        _TxtSoldiersNum.text = string.Format(_SoldierStringFormat, soldierNum);

        if (!_IsSetTarget)
        {
            if (!_BtnCancel.gameObject.activeSelf) _BtnCancel.gameObject.SetActive(true);
            _BtnCancel.transform.position = _BtnTarget.transform.position;
        }

        _IsSetOrigin = true;
    }

    public void SetTargetInfo(Vector3Int pos)
    {
        if (!_BtnTarget.gameObject.activeSelf)
        {
            _BtnTarget.gameObject.SetActive(true);

            _TxtTargetPosition.gameObject.SetActive(true);
        }
        _TxtTargetPosition.text = string.Format(_PositionStringFormat, pos.x, pos.z);

        if (!_IsSetOrigin)
        {
            if (!_BtnCancel.gameObject.activeSelf) _BtnCancel.gameObject.SetActive(true);
            _BtnCancel.transform.position = _BtnOrigin.transform.position;
        }

        _IsSetTarget = false;   
    }

    public void DelayClose(float time)
    {
        _CloseTime = time + Time.time;
        _IsDelayClose = true;
    }

    private void Update()
    {
        if (_IsDelayClose)
        {
            if (Time.time > _CloseTime)
            {
                _IsDelayClose = false;
                CloseUIForm();
            }      
        }
    }
}
