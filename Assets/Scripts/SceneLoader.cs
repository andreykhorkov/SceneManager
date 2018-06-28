using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private SceneADependencies sceneADependenciesData;

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
            sceneADependenciesData = gameObject.AddComponent<SceneADependencies>();
            SceneManager.LoadScene("A");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(sceneADependenciesData.SceneBData.BData);
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

        for (int i = 0; i < sceneData.Scenes.Count; i++)
        {
            var name = sceneData.Scenes[i];
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
    }
}