/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-16:42:05
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneState : ISceneState
{
    private ISceneState targetState = null;
    private AsyncOperation targetAsyncOperation = null;
    private Func<AsyncOperation> loadSceneAction = null;
    private Text textProgress = null;
    private float isDelayToLoadTime = 0;

    public LoadingSceneState(ISceneStateController controller) : base("Loading", controller) { }

    public override void Enter()
    {
        isDelayToLoadTime = 0;
        textProgress = UITool.GetUIComponent<Text>("textProgressCount");
        textProgress.text = "0%";
        if (loadSceneAction != null)
        {
            targetAsyncOperation = loadSceneAction();
            targetAsyncOperation.allowSceneActivation = false;
        }
    }

    public void SetInfos(ISceneState target, Func<AsyncOperation> loadSceneAction)
    {
        this.targetState = target;
        this.loadSceneAction = loadSceneAction;
    }

    public override void Update()
    {
        if (targetAsyncOperation != null && Mathf.Approximately(isDelayToLoadTime, 0f))
        {
            if (targetAsyncOperation.progress < 0.89)
            {
                textProgress.text = (int)(targetAsyncOperation.progress * 100) + "%";
            }
            else
            {
                textProgress.text =  "100%";
                isDelayToLoadTime = Time.time + 1;
            }
        }

        if (isDelayToLoadTime > 0 && isDelayToLoadTime < Time.time)
        {
            isDelayToLoadTime = 0;         
            loadSceneAction = null;
            targetAsyncOperation.allowSceneActivation = true;
            targetAsyncOperation = null;
            StateController.SetState(targetState, "");
            targetState = null;
        }
    }
}

