using System;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private ISceneStateController m_SceneStateController;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_SceneStateController = new SceneStateController();
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    private void Start()
    {
        m_SceneStateController.SetState(new StartSceneState(m_SceneStateController), "");
    }


    private void Update()
    {
        m_SceneStateController.StateUpdate();
    }
}
