using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{

    [SerializeField, AssetFullPathGetter] private List<string> scenes;

    public List<string> Scenes { get { return scenes; } }

}
