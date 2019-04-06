using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainBaseDrawEvent : MonoBehaviour, IDragHandler, IDropHandler
{

    public bool isMainBase = true;
    public MainBaseVO mainBaseVO = null;

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;

        var sc = go.GetComponent<MainBaseDrawEvent>();
        if (sc.isMainBase && isMainBase)
        {
            var threeCmdParams = TreeMsgParamsPool<MainBaseVO, MainBaseVO, int>.Instance.Pop();
            threeCmdParams.InitParams(sc.mainBaseVO, mainBaseVO, 20);
            GameFacade.Instance.SendNotification(GlobalSetting.Cmd_MoveTroops, threeCmdParams);
        } 
    }
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //IBeginDragHandler
    //}
    public void OnDrag(PointerEventData eventData)
    {
    }
}
