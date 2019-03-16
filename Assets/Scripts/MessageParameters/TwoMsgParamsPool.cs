/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-14:21:48
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using System.Collections.Generic;

public class TwoMsgParamsPool<TFirst, TSecond> : MsgParamPool<TwoMsgParamsPool<TFirst, TSecond>>
{
    public TwoMsgParamsPool()
    {
        _Params = new Stack<TwoMsgParams<TFirst, TSecond>>() as Stack<object>;
    }

    public TwoMsgParams<TFirst, TSecond> Pop()
    {
        if (_Params.Count > 0)
            return _Params.Pop() as TwoMsgParams<TFirst, TSecond>;
        else
            return new TwoMsgParams<TFirst, TSecond>();
    }

    public void Push(TwoMsgParams<TFirst, TSecond> cmdParam)
    {
        _Params.Push(cmdParam);
    }
}

