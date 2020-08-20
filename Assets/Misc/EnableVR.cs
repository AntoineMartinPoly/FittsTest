using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableVR : MonoBehaviour
{
    void Awake()
    {
        print("enabled xr");
        UnityEngine.XR.XRSettings.enabled = true;
    }
}
