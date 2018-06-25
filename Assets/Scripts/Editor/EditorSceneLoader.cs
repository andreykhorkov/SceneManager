using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class EditorSceneLoader
{
    static EditorSceneLoader()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
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
            var subScene = EditorSceneManager.OpenScene(sceneData.DependentScenes.Scenes[i], OpenSceneMode.Additive);
            SceneManager.SetActiveScene(subScene);
        }
    }
}
