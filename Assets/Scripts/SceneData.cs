using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{

    [SerializeField] private DependentScenes dependentScenes;

    public DependentScenes DependentScenes { get { return dependentScenes; } }

}
