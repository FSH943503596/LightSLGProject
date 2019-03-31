using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainBaseDrawEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{


    //public void OnInitializePotentialDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnInitializePotentialDrag " + name);
    //}
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop " + name);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag " + name);

    }

    public void OnDrag(PointerEventData eventData)
    {
    }
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnEndDrag " + name);

    //}

}
