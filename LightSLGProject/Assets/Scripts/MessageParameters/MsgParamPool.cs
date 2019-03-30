/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-14:24:13
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using System.Collections.Generic;

public class MsgParamPool<T> where T : new()
{
    public static readonly T Instance;

    static MsgParamPool()
    {
        Instance = new T();
    }

    protected Stack<object> _Params;

    public void Clear()
    {
        _Params.Clear();
    }
}

