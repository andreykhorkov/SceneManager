using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
[CustomEditor(typeof(Temp))]
public class EditorSceneLoader : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("suka", EditorStyles.miniButton))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/1.unity", OpenSceneMode.Additive);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
    }
}

[InitializeOnLoad]
public static class LatestScenes
{
    private static string currentScene;

    static LatestScenes()
    {
        currentScene = EditorApplication.currentScene;
        EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
    }

    private static void hierarchyWindowChanged()
    {
        if (currentScene != EditorApplication.currentScene)
        {
            //a scene change has happened
            Debug.Log("Last Scene: " + currentScene);
            currentScene = EditorApplication.currentScene;
        }
    }
}
