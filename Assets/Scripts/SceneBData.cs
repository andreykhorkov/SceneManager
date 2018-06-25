using UnityEngine;

public class SceneBData : MonoBehaviour
{

	public string BData { get { return "BBB"; } }

    void Start()
    {
        var sceneADependencies = SceneLoader.Instance.GetComponent<SceneADependencies>();
        sceneADependencies.Resolve(this);
    }

}
