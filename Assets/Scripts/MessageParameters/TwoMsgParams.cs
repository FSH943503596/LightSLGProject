/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-13:59:31
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class TwoMsgParams<TFirst, TSecond>
{
    private TFirst _First;
    private TSecond _Second;
    public TFirst first => _First;
    public TSecond second => _Second;

    public TwoMsgParams() { }
    public TwoMsgParams(TFirst first, TSecond second)
    {
        InitParams(first, second);
    }

    public void InitParams(TFirst first, TSecond second)
    {
        _First = first;
        _Second = second;
    }
}

