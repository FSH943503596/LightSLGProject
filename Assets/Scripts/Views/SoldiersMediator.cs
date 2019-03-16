/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-16:17:52
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Patterns;
using System;
using UnityEngine;

public class SoldiersMediator : Mediator
{
    new public const string NAME = "SoldiersMediator";

    private Transform _SoldiersParent;

    public SoldiersMediator() : base(NAME)
    {
        _SoldiersParent = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_SOLDIERS_PARENT_NAME).transform;
    }
}

