using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HierarchyMenuHelp : Editor
{
    [MenuItem("GameObject/UI/Open___UITest")]
    public static void RunUITestSence()
    {
        EditorSceneManager.OpenScene("Assets/Z-Test/UITest.unity");
    }
    [MenuItem("GameObject/UI/Open___Start")]
    public static void RunStartSence()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Start.unity");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RunUITestSence();
        RunStartSence();
    }

}
