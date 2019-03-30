/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-28-14:03:20
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine;

public class ExitGameCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        Application.Quit();
    }
}

