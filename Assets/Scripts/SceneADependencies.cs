using UnityEngine;

public class SceneADependencies : MonoBehaviour
{
    public SceneBData SceneBData { get; protected set; }

    public void SetData(SceneBData sceneData)
    {
        SceneBData = sceneData;
    }
}
