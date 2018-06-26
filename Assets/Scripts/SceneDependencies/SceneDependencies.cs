using UnityEngine;

public abstract class SceneDependencies<TSceneData> : MonoBehaviour where TSceneData : SceneData, new ()
{
    public TSceneData SceneData { get; protected set; }

    public void SetData(TSceneData sceneData)
    {
        SceneData = sceneData;
    }
}
