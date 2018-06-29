using UnityEngine;
using Zenject;

public class MainManager : MonoBehaviour
{
    public string str = "suka";

    private void Awake() {
        Debug.Log("MainManager Awake");
    }

    private void Start() {
        Debug.Log("MainManager Start");
    }


}