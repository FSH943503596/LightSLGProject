/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-15:28:45
 *	Vertion: 1.0	
 *
 *	Description: 场景基类
 *	    定义场景进入，更新，退出行为
 *	    定义场景名称
*/

public abstract class ISceneState
{
    public string StateName { get; set; }
    protected ISceneStateController StateController { get; set; }
    public ISceneState(string name, ISceneStateController controller)
    {
        this.StateName = name;
        this.StateController = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

    public override string ToString()
    {
        return string.Format("I_SceneState:StateName={0}", StateName);
    }
}

