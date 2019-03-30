/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-04-17:40:01
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine;

public class ShowConstructionCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        //IBuilding building = notification as IBuilding;
        //if (building == null) return;

        //switch (building.PrefabName)
        //{
        //    case "MainBase":
        //    case "SubBase":
        //        break;
        //    case "FarmLand":
        //        break;
        //    case "GoldMine":
        //        break;
        //    case "MilitaryCamp":
        //        break;
        //    default:
        //        break;
        //}

        Transform buildingTF = notification.Body as Transform;
        if (buildingTF)
        {
            BattleManager.Instance.ShowConstraction(buildingTF);
        }
    }
}

