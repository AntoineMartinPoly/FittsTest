using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ShowValue : MonoBehaviour
{
    private Text text;
    public PostProcessVolume vol;
    //private DepthOfField dof;
    private ColorGrading cg;
    //private ColorRGB colorRGB;

    private void Start()
    {
        text = GetComponent<Text>();
        //vol.profile.TryGetSettings(out dof);
        vol.profile.TryGetSettings(out cg);
        //colorRGB = GameObject.Find("GraphicScript").GetComponent<ColorRGB>();                
    }

    void Update()
    {
        text.text = ""
        + "\n highest value (y axis) : " + cg.hueVsHueCurve.value.curve.keys[76].value;
        //+ "\n Brightness : " + cg.brightness.value;
        //+ "\n Aperture : " + dof.aperture.value
        //+ "\n Focus distance : " + dof.focusDistance.value
        //+ "\n Focal Length : " + dof.focalLength.value
        //+ "\n Blur Size : " + dof.kernelSize.value;
        //+ "\n fps : " + (1.0f / Time.deltaTime)
        //+ "\n Fixed Delta Time : " + Time.fixedDeltaTime
        //+ "\n Maximum Delta Time : " + Time.maximumDeltaTime;
        //+ "\n Scene Resolution Scale : " + Valve.VR.SteamVR_Camera.sceneResolutionScale;
        //               + "\n RGB interval  : " + (colorRGB.interval * 255);
        //text.text += "\n Red non valid value : ";
        //foreach (int value in colorRGB.redIntervalToRemove)
        //{
        //    text.text += value + ", ";
        //}
        //text.text += "\n Green non valid value : ";
        //foreach (int value in colorRGB.greenIntervalToRemove)
        //{
        //    text.text += value + ", ";
        //}
        //text.text += "\n Blue non valid value : ";
        //foreach (int value in colorRGB.blueIntervalToRemove)
        //{
        //    text.text += value + ", ";
        //}
    }
}
