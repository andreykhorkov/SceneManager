using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DependentScenes")]
public class DependentScenes : ScriptableObject
{

    [SerializeField, AssetFullPathGetter] private List<string> scenes;

    public List<string> Scenes { get { return scenes; } }
}
