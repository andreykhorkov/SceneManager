using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TSScene
{
    [AssetFullPathGetter] public string sceneId;
    public List<TSScene> subScenes;
}

[CreateAssetMenu(fileName = "TSSceneDefinitions")]
public class TSSceneDefinitions : ScriptableObject
{
    [AssetFullPathGetter] public string rootScene;
    public List<TSScene> loadableScenes;
}