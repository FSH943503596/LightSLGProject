/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-13:59:31
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class ThreeMsgParams<TFirst, TSecond, TThird>
{
    private TFirst _First;
    private TSecond _Second;
    private TThird _Third;

    public ThreeMsgParams() { }
    public ThreeMsgParams(TFirst first, TSecond second, TThird third)
    {
        InitParams(first, second, third);
    }

    public TFirst First => _First;
    public TSecond Second => _Second;
    public TThird Third => _Third;

    public void InitParams(TFirst first, TSecond second, TThird third)
    {
        _First = first;
        _Second = second;
        _Third = third;
    }
}

