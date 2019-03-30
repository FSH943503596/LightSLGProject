/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-14:42:08
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System.Collections.Generic;

public class TreeMsgParamsPool<TFirst, TSecond, TThird> : MsgParamPool<TreeMsgParamsPool<TFirst, TSecond, TThird>>
{
    public TreeMsgParamsPool()
    {
        _Params = new Stack<object>();
    }

    public ThreeMsgParams<TFirst, TSecond, TThird> Pop()
    {
        if (_Params.Count > 0)
            return _Params.Pop() as ThreeMsgParams<TFirst, TSecond, TThird>;
        else
            return new ThreeMsgParams<TFirst, TSecond, TThird>();
    }

    public void Push(ThreeMsgParams<TFirst, TSecond, TThird> cmdParam)
    {
        _Params.Push(cmdParam);
    }			
}

