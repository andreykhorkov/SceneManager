public class SceneBData : SceneData
{

	public string BData { get { return "BBB"; } }

    void Start()
    {
        var sceneADependencies = SceneLoader.Instance.GetComponent<SceneADependencies>();
        sceneADependencies.SetData(this);
    }

}
