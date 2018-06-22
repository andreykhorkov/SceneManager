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

        EditorSceneManager.OpenScene("Assets/Scenes/1.unity", OpenSceneMode.Additive);
    }
}
