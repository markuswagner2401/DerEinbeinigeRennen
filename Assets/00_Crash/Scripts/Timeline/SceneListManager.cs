using UnityEngine;
using System.Collections.Generic;

public class SceneListManager : MonoBehaviour
{
    [SerializeField]
    private List<string> sceneNames;
    
    public bool CheckIfSceneExists(string sceneName)
    {
        return sceneNames.Contains(sceneName);
    }
}

