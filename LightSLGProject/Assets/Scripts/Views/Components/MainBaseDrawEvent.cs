using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainBaseDrawEvent : MonoBehaviour, IDragHandler, IDropHandler
{
    public MainBaseVO mainBaseVO = null;

    public void OnDrop(PointerEventData eventData)
    {

        Debug.Log("OnDrop 进入滑屏");
        var go = eventData.pointerDrag;

        var sc = go.GetComponent<MainBaseDrawEvent>();
        if (sc.mainBaseVO.buildingType == E_Building.MainBase && mainBaseVO.buildingType == E_Building.MainBase && sc.mainBaseVO.ower.IsUser)
        {
            var threeCmdParams = TreeMsgParamsPool<MainBaseVO, MainBaseVO, int>.Instance.Pop();
            threeCmdParams.InitParams(sc.mainBaseVO, mainBaseVO, sc.mainBaseVO.soldierNum / 2);
            GameFacade.Instance.SendNotification(GlobalSetting.Cmd_MoveTroops, threeCmdParams);
            Debug.Log("OnDrop 发送出兵");

        } 
    }
    public void OnDrag(PointerEventData eventData)
    {
    }
}
