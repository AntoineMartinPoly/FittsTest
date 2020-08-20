using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ContrastScale : MonoBehaviour
{
    public Toggle displayContrast;

    private void Start()
    {
        displayContrast.onValueChanged.AddListener((newValue) => { MoveContrastScale(newValue); });
    }
    public static void MoveContrastScale(bool moveCloser)
    {
        GameObject.Find("ContrastScale").transform.position = Camera.main.transform.position + (moveCloser ? Vector3.forward : Vector3.forward * 10);
    }
}
