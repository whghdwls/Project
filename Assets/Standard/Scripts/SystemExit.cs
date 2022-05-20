using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemExit : MonoBehaviour
{
    void Start()
    {    
    }

    void Update()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로
        #endif
        Application.Quit();
        
    }
}
