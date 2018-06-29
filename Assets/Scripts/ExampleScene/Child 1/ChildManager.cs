using UnityEngine;
using Zenject;

public class ChildManager : MonoBehaviour {

    private MainManager _mainManager;
    private bool _canRun = false;

    [Inject]
    private SceneLoader sceneLoader;

    [InjectOptional]
    public MainManager MainManager
    {
        get
        {
            return _mainManager;
        }
        set
        {
            Debug.Log("ChildManager injected with MainManager");
            _mainManager = value;
            _canRun = true;
        }
    }

    private void Awake()
    {
        Debug.Log("ChildManager Awake");
    }

    private void Start()
    {
        Debug.Log("ChildManager Start");
    }
}