using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class CompositeScene
{
    [AssetFullPathGetter, SerializeField] private string scenePath;
    [SerializeField] private List<CompositeScene> subScenes;

    public string ScenePath { get { return scenePath; } }
    public List<CompositeScene> SubScenes { get { return subScenes; } } 
}

[CreateAssetMenu(fileName = "SceneDefinitions")]
public class SceneDefinitions : SerializedScriptableObject
{
    [AssetFullPathGetter, SerializeField] private string rootScenePath;
    [SerializeField] private List<CompositeScene> loadableScenes;

    public string RootScenePath { get { return rootScenePath; } }
    public List<CompositeScene> LoadableScenes { get { return loadableScenes; } }
}