using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class Temp
{
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

    public static void LoadSceneAndItsSubscenes(CompositeScene parentCompositeScene, SceneDefinitions sceneDefinitions)
    {
        SceneManager.LoadScene(sceneDefinitions.RootScenePath, LoadSceneMode.Single);
        SceneManager.LoadScene(parentCompositeScene.ScenePath);
        OpenSubscenesRecursively(parentCompositeScene);
    }

    public static void OpenSubscenesRecursively(CompositeScene scene)
    {
        for (int i = 0; i < scene.SubScenes.Count; i++)
        {
            SceneManager.LoadScene(scene.SubScenes[i].ScenePath);
            OpenSubscenesRecursively(scene.SubScenes[i]);
        }
    }

    public static CompositeScene TryFindSceneRecursively(string path, List<CompositeScene> scenes)
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

            var neededScene = TryFindSceneRecursively(path, compositeScene.SubScenes);

            if (neededScene != null)
            {
                return neededScene;
            }
        }

        return null;
    }
}
