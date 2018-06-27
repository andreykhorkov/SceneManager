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
            //Temp.LoadSceneAndItsSubscenes(sceneDefinitions.LoadableScenes[0], sceneDefinitions);
            SceneManager.LoadScene("Assets/TSChild_1");
        }

    }


}