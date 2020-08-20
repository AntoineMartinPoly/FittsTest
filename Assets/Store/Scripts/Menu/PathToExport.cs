using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class attached to the Path section of the menu
public class PathToExport : MonoBehaviour
{
    public InputField pathInput;
    void Start()
    {
        pathInput.onValueChanged.AddListener((newPath) => { FittsExport.path = newPath; });
    }
}
