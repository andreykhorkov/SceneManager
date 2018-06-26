using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhilSceneManager : MonoBehaviour
{
    public TSSceneDefinitions sceneDefinitions;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene()
    {
        
    }

}
