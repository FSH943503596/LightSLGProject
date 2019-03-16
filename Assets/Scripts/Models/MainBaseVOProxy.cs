/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-25-14:24:39
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class MainBaseVOProxy : Proxy
{
    new public const string NAME = "MainBaseProxy";

    private IList<MainBaseVO> mainBases
    {
        get {
            return base.Data as IList<MainBaseVO>;
        } 
    }

    public MainBaseVOProxy() : base(NAME, new List<MainBaseVO>())
    {
    }

    public MainBaseVO CreateMainBase(PlayerVO owner) {

        MainBaseVO mainBaseVO = new MainBaseVO(owner);

        return mainBaseVO;
    }
}

