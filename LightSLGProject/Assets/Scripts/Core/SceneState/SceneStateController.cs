/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-13-15:45:43
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateController : ISceneStateController
{
    private ISceneState state;
    private bool isRunEnter = false;
    private bool isLoading = false;
    private bool isAsync = false;
    private LoadSceneMode setStateMode = LoadSceneMode.Single;
    private int waitForUnloadIndex = -1;

    public SceneStateController()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoading = false;
        setStateMode = LoadSceneMode.Single;
        if (isAsync && scene.name != "Loading")
        {
            isAsync = false;
            SceneManager.UnloadSceneAsync("Loading");
        }
        if (scene.name == "Loading" && waitForUnloadIndex > -1)
        {
            SceneManager.UnloadSceneAsync(waitForUnloadIndex);
            waitForUnloadIndex = -1;
        }
    }

    public void SetState(ISceneState state, string loadSceneName)
    {
        isRunEnter = false;

        LoadScene(loadSceneName);

        if (this.state != null)
            this.state.Exit();

        this.state = state;
    }

    public void SetStateAsync(ISceneState state, string loadSceneName)
    {
        isAsync = true;

        LoadingSceneState loadingSceneState = new LoadingSceneState(this);
        setStateMode = LoadSceneMode.Additive;
        waitForUnloadIndex = SceneManager.GetActiveScene().buildIndex;
        SetState(loadingSceneState, "Loading");

        loadingSceneState.SetInfos(state, ()=>LoadSceneAsync(loadSceneName));
    }

    private void LoadScene(string loadSceneName)
    {
        if (loadSceneName == null || loadSceneName.Length == 0)
            return;
        isLoading = true;
        SceneManager.LoadScene(loadSceneName, setStateMode);
    }

    private AsyncOperation LoadSceneAsync(string loadSceneName)
    {
        if (loadSceneName == null || loadSceneName.Length == 0)
            return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName,LoadSceneMode.Additive);

        return asyncOperation;
    }

    public void StateUpdate()
    {
        if (isLoading) return;

        if (state != null && isRunEnter == false)
        {
            state.Enter();
            isRunEnter = true;
        }

        if (state != null)
            state.Update();
    }
}

