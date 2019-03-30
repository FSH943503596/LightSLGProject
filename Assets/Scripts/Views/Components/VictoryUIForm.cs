/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-28-17:44:03
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using SUIFW;
using System;
using UnityEngine.EventSystems;

public class VictoryUIForm : BaseUIForm, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        CloseUIForm();
    }
}

