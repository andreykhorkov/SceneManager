using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : MonoInstaller<SceneLoader>
{
    [SerializeField] private SceneDefinitions sceneDefinitions;
    
    public SceneDefinitions SceneDefinitions { get { return sceneDefinitions; } }

    private ZenjectSceneLoader _loader;

    [Inject]
    public void Init(ZenjectSceneLoader loader)
    {
        _loader = loader;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(this).AsSingle();
    }

    void Awake()
    {
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

#if UNITY_EDITOR
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
#endif

    public void LoadSceneAndItsSubscenes(CompositeScene parentCompositeScene)
    {
        var mainScene = SceneManager.GetSceneByPath(sceneDefinitions.RootScenePath);

        if (!mainScene.IsValid())
        {
            _loader.LoadScene(sceneDefinitions.RootScenePath, LoadSceneMode.Single, ExtraBindings);
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

        _loader.LoadScene(parentCompositeScene.ScenePath, LoadSceneMode.Additive, null, LoadSceneRelationship.Child);
        OpenSubscenesRecursively(parentCompositeScene);
    }

    private void ExtraBindings(DiContainer diContainer)
    {
        diContainer.BindInstance(this).AsSingle();
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
            _loader.LoadScene(scene.SubScenes[i].ScenePath, LoadSceneMode.Additive, null, LoadSceneRelationship.Child);
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