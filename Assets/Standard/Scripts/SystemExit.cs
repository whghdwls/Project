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
            UnityEditor.EditorApplication.isPlaying = false; //play��带 false��
        #endif
        Application.Quit();
        
    }
}
