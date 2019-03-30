/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-16:33:23
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;

public class BattleSceneState : ISceneState
{
    IBattleManager gameBattleManager = null;

    public BattleSceneState(ISceneStateController controller, IBattleManager battleManager) : base("Battle", controller)
    {
        gameBattleManager = battleManager;
    }

    public override void Enter()
    {
        gameBattleManager.Initinal();      
    }

    public override void Update()
    {
        gameBattleManager.Update();

        if (gameBattleManager.isBattleOver)
        {
            StateController.SetStateAsync(new MainSceneState(StateController), "Main");
        }
    }

    public override void Exit()
    {
        gameBattleManager.Release();
    }
}

