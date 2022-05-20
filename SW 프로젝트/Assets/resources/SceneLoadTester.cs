using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadingSceneController2.Instance.LoadScene("InGame");
    }
}
