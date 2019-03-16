/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-15:57:29
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;

public class HUDConstruction : MonoBehaviour
{
    [SerializeField] private Transform _BuildingParent;
    [SerializeField] private Renderer _Renderer;

    private bool _IsCanConfirm = false;

    public void SetIsCanConfirm(bool isCanConfirm)
    {
        _IsCanConfirm = isCanConfirm;
        if (_IsCanConfirm)
        {
            _Renderer.material.color = Color.green;
        }
        else
        {
            _Renderer.material.color = Color.red;
        }
    }
}
