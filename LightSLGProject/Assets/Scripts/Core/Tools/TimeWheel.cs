/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-14-14:46:50
 *	Vertion: 1.0	
 *
 *	Description:
 *      轮盘计时器, 暂时不用后续完成
*/

using System;

public static class TimeWheel
{
    private static Node[] nodeWheel;

    private static void Update()
    {

    }

    private static void FixUpdate()
    {

    }

    private class Node
    {
        public short Count;
        public Node PreNode;
        public Node NextNode;
    }
}

