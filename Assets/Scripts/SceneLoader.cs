using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneDefinitions sceneDefinitions;
    
    public SceneDefinitions SceneDefinitions { get { return sceneDefinitions; } }

    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadSceneAndItsSubscenes(sceneDefinitions.LoadableScenes[1].ScenePath);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadSceneAndItsSubscenes(sceneDefinitions.LoadableScenes[0].ScenePath);
        }
    }

    public static void LoadSceneAndItsSubscenesEditor(CompositeScene parentCompositeScene, SceneDefinitions sceneDefinitions)
    {
        //load root scene additively
        EditorSceneManager.OpenScene(sceneDefinitions.RootScenePath, OpenSceneMode.Single);

        //unload previous scenes to avoid nullrefs

        EditorSceneManager.OpenScene(parentCompositeScene.ScenePath, OpenSceneMode.Additive);

        OpenSubScenesRecursivelyEditor(parentCompositeScene);
    }

    public static void OpenSubScenesRecursivelyEditor(CompositeScene scene)
    {
        foreach (var child in scene.SubScenes)
        {
            EditorSceneManager.OpenScene(child.ScenePath, OpenSceneMode.Additive);
            OpenSubScenesRecursivelyEditor(child);
        }
    }

    public void LoadSceneAndItsSubscenes(CompositeScene parentCompositeScene)
    {
        var mainScene = SceneManager.GetSceneByPath(sceneDefinitions.RootScenePath);

        if (!mainScene.IsValid())
        {
            SceneManager.LoadScene(sceneDefinitions.RootScenePath, LoadSceneMode.Single);
        }
        else
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene != mainScene)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }

        SceneManager.LoadScene(parentCompositeScene.ScenePath, LoadSceneMode.Additive);
        OpenSubscenesRecursively(parentCompositeScene);
    }

    public void LoadSceneAndItsSubscenes(string parentSceneName)
    {
        var compositeScene = FindSceneRecursively(parentSceneName, sceneDefinitions.LoadableScenes);

        if (compositeScene == null)
        {
            return;
        }

        LoadSceneAndItsSubscenes(compositeScene);
    }

    public void OpenSubscenesRecursively(CompositeScene scene)
    {
        for (int i = 0; i < scene.SubScenes.Count; i++)
        {
            SceneManager.LoadScene(scene.SubScenes[i].ScenePath, LoadSceneMode.Additive);
            OpenSubscenesRecursively(scene.SubScenes[i]);
        }
    }

    public static CompositeScene FindSceneRecursively(string path, List<CompositeScene> scenes)
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            var compositeScene = scenes[i];

            if (string.Equals(path, compositeScene.ScenePath))
            {
                return compositeScene;
            }
        }

        for (int i = 0; i < scenes.Count; i++)
        {
            var compositeScene = scenes[i];

            var neededScene = FindSceneRecursively(path, compositeScene.SubScenes);

            if (neededScene != null)
            {
                return neededScene;
            }
        }

        return null;
    }
}