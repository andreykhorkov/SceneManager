using UnityEngine;

public class SceneADependencies : MonoBehaviour
{
    public SceneBData SceneBData { get; private set; }

    public void Resolve(SceneBData sceneBData)
    {
        SceneBData = sceneBData;
    }

}
