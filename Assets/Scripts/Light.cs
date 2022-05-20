using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    internal float shadowStrength;
    internal float shadowBias;
    [SerializeField]
    GameObject FlashLight;

    private bool FlashLightActive = false;
    // Start is called before the first frame update
    void Start()
    {
        FlashLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(FlashLightActive == false)
            {
                FlashLight.gameObject.SetActive(true);
                FlashLightActive = true;
            }
            else
            {
                FlashLight.gameObject.SetActive(false);
                FlashLightActive = false;
            }
        }
    }
}