using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class EditorSceneLoader
{
    private static Scene lastScene;

    static EditorSceneLoader()
    {
        lastScene = SceneManager.GetActiveScene();
        EditorApplication.hierarchyChanged += hierarchyWindowChanged;
    }

    private static void hierarchyWindowChanged()
    {
        var currentScene = SceneManager.GetActiveScene();

        if (lastScene != currentScene)
        {
            OnSceneLoded(currentScene);
        }
    }

    private static void OnSceneLoded(Scene scene)
    {
        Debug.LogFormat("{0} scene loaded", scene.name);
        lastScene = scene;

        var rootObjs = lastScene.GetRootGameObjects();
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
