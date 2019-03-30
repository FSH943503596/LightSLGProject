/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-07-15:00:39
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

public interface IActorAgent
{
    
    //设置目标
    void SetTarget(UnityEngine.Vector3 target);

    //到达目标消息处理
    void AddCompleteListener(System.Action completeCallBack);
    void RemoveCompleteListener(System.Action completeCallBack);
    void ClearCompleteEvent();
}

