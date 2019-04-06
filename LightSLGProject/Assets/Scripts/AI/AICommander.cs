/*
 *	Author:	贾树永
 *	CreateTime:	2019-04-02-13:57:27
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class AICommander
{
    public Action<string, object> sender;

    public void MoveTroops(MainBaseVO from, MainBaseVO end)
    {
        if (from != null && end != null && !from.Equals(end))
        {
            int soldierCount = from.soldierNum / 2;
            if (soldierCount <= 0) return;
            var msgParam = TreeMsgParamsPool<MainBaseVO, MainBaseVO, int>.Instance.Pop();
            msgParam.InitParams(from, end, soldierCount);
            sender(GlobalSetting.Cmd_MoveTroops, msgParam);
        }
    }
}

