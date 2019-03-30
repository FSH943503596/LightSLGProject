/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-20-11:34:55
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using SUIFW;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleInfoUIForm : BaseUIForm
{
    [SerializeField] private Image _ImgPlayerFlag;
    [SerializeField] private Text _TxtMainBaseCount;
    [SerializeField] private Text _TxtSoldierCount;
    [SerializeField] private Slider _SliderGold;
    [SerializeField] private Slider _SliderGrain;
    [SerializeField] private Text _TxtGoldNumber;
    [SerializeField] private Text _TxtGrainNumber;

    private Action _PreRanderAction;

    private string _NumberFormat = "{0}/{1}";

    public Action preRanderAction { set => _PreRanderAction = value; }

    public void SetGoldInfo(int current, int maxValue)
    {
        _SliderGold.maxValue = maxValue;
        _SliderGold.value = current;
        _TxtGoldNumber.text = string.Format(_NumberFormat, current, maxValue);
    }

    public void SetGrainInfo(int current, int maxValue)
    {
        _SliderGrain.maxValue = maxValue;
        _SliderGrain.value = current;
        _TxtGrainNumber.text = string.Format(_NumberFormat, current, maxValue);
    }

    public void SetMainBaseCount(int count)
    {
        _TxtMainBaseCount.text = count.ToString();
    }

    public void SetSoldierCount(int current, int maxValue)
    {
        _TxtSoldierCount.text = string.Format(_NumberFormat, current, maxValue);
    }

    public void SetPlayerFlag(int colorIndex)
    {
        colorIndex = colorIndex < GlobalSetting.PLAYER_COLOR_LIST.Count ? colorIndex : GlobalSetting.PLAYER_COLOR_LIST.Count - 1;
        _ImgPlayerFlag.color = GlobalSetting.PLAYER_COLOR_LIST[colorIndex];
    }

    public void SetDefaultInfo()
    {
        SetGoldInfo(0, 0);
        SetGrainInfo(0, 0);
        SetMainBaseCount(1);
        SetSoldierCount(0, 0);
        SetPlayerFlag(0);
    }

    private void OnGUI()
    {
        if (_PreRanderAction != null)
        {
            _PreRanderAction();
        }
    }
}
