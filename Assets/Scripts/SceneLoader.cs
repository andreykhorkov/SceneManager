﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public SceneADependencies dep;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            dep = gameObject.AddComponent<SceneADependencies>();
            SceneManager.LoadScene("A");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(dep.SceneBData.BData);
        }
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