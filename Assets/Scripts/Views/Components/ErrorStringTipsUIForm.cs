using SUIFW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorStringTipsUIForm : TipsBaseUIForm
{
    [SerializeField] private Text _TxtErrorString;
    public void SetMsg(string errorMsg)
    {
        _TxtErrorString.text = errorMsg;
    }
}
