/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-15:37:28
 *	Vertion: 1.0	
 *
 *	Description: 场景管理器接口
 *	    定义场景切换方法，场景更新方法
 *
*/

public interface ISceneStateController
{
    /// <summary>
    /// 设置场景状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="loadSceneName"></param>
    void SetState(ISceneState state, string loadSceneName);

    void SetStateAsync(ISceneState state, string loadSceneName);

    /// <summary>
    /// 更新场景
    /// </summary>
    void StateUpdate();
}

