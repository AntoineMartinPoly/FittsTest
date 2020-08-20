using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableVR : MonoBehaviour
{
    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = false;
    }
}
