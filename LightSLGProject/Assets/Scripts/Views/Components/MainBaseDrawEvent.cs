using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainBaseDrawEvent : MonoBehaviour, IDragHandler, IDropHandler
{
    public MainBaseVO mainBaseVO = null;

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;

        var sc = go.GetComponent<MainBaseDrawEvent>();
        if (sc.mainBaseVO.buildingType == E_Building.MainBase && mainBaseVO.buildingType == E_Building.MainBase)
        {
            var threeCmdParams = TreeMsgParamsPool<MainBaseVO, MainBaseVO, int>.Instance.Pop();
            threeCmdParams.InitParams(sc.mainBaseVO, mainBaseVO, 20);
            GameFacade.Instance.SendNotification(GlobalSetting.Cmd_MoveTroops, threeCmdParams);
        } 
    }
    public void OnDrag(PointerEventData eventData)
    {
    }
}
