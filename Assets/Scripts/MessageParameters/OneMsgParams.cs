/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-13:59:31
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class OneMsgParams<T> where T: struct
{
    private T _Parameter;
    public T parameter => _Parameter;

    public OneMsgParams() { }
    public OneMsgParams(T parameter)
    {
        InitParams(parameter);
    }

    public void InitParams(T parameter)
    {
        _Parameter = parameter;
    }
}

