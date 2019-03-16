/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-14-15:28:47
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using PureMVC.Interfaces;
using System;

public abstract class IBattleSystem<T> where T: IBattleManager
{
    protected T battleManager;
    protected IFacade facade;
    public IBattleSystem(IBattleManager mgr)
    {
        battleManager = (T)mgr;
        facade = GameFacade.Instance;
    }

    public virtual void Initialize()
    {
        
    }
    public virtual void Release() { }
    public virtual void Update() { }
}

