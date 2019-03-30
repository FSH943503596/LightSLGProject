/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-28-16:41:13
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using SUIFW;
using UnityEngine;

public class TipsBaseUIForm : BaseUIForm
{
    [SerializeField] private float _ShowTime;

    private float _CloseTime;

    public override void Display()
    {
        base.Display();
        _CloseTime = Time.time + _ShowTime;
    }
    void Update()
    {
        if (Time.time > _CloseTime)
        {
            CloseUIForm();
        }
    }
}
