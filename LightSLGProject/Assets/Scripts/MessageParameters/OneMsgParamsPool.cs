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

public class OneMsgParamsPool<T> : MsgParamPool<OneMsgParamsPool<T>> where T : struct
{
    public OneMsgParamsPool()
    {
        _Params = new Stack<object>();
    }

    public OneMsgParams<T> Pop()
    {
        if (_Params.Count > 0)
            return _Params.Pop() as OneMsgParams<T>;
        else
            return new OneMsgParams<T>();
    }

    public void Push(OneMsgParams<T> cmdParam)
    {
        _Params.Push(cmdParam);
    }
}

