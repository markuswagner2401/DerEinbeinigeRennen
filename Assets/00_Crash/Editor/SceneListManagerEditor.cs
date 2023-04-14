using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SceneListManager))]
public class SceneListManagerEditor : Editor, IActiveBuildTargetChanged
{
    private SceneListManager sceneListManager;

    private void OnEnable()
    {
        sceneListManager = (SceneListManager)target;
        UpdateSceneNames();
    }

    public int callbackOrder { get { return 0; } }

    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        UpdateSceneNames();
    }

    private void UpdateSceneNames()
{
    List<string> sceneNames = new List<string>();

    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
    {
        if (EditorBuildSettings.scenes[i].enabled) // Only add enabled scenes
        {
            string scenePath = EditorBuildSettings.scenes[i].path;
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }
    }

    SerializedProperty sceneNamesProperty = serializedObject.FindProperty("sceneNames");
    sceneNamesProperty.ClearArray();

    for (int i = 0; i < sceneNames.Count; i++)
    {
        sceneNamesProperty.InsertArrayElementAtIndex(i);
        sceneNamesProperty.GetArrayElementAtIndex(i).stringValue = sceneNames[i];
    }

    serializedObject.ApplyModifiedProperties();
}
}