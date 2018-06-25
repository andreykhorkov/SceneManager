using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneLoader
{
    static SceneLoader()
    {
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }

    private static void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var rootObjs = scene.GetRootGameObjects();
        SceneData sceneData = null;

        for (int i = 0; i < rootObjs.Length; i++)
        {
            sceneData = rootObjs[i].GetComponent<SceneData>();

            if (sceneData != null)
            {
                break;
            }
        }

        if (sceneData == null)
        {
            return;
        }

        for (int i = 0; i < sceneData.DependentScenes.Scenes.Count; i++)
        {
            SceneManager.LoadScene(sceneData.DependentScenes.Scenes[i], LoadSceneMode.Additive);
        }
    }
}