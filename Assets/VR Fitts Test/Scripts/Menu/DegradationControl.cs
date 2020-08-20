using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

// Class attached to the Degradation Control section of the menu
public class DegradationControl : MonoBehaviour
{
    public Text contrastValue;
    public Text colorValue;
    public Text resolutionValue;
    public Text fovValue;
    public GameObject fitts;

    public void SaveState()
    {
        FittsExport.SaveState(contrastValue.text, fitts.GetComponent<ColorHUE>().GetKeyframes(), resolutionValue.text, fovValue.text);
        TurnOffAllDegradation();
    }

    public void ResetDegradation()
    {
        fitts.GetComponent<Contrast>().ValueToDegrade = 0;
        fitts.GetComponent<ColorHUE>().ResetKeyFrames();
        fitts.GetComponent<Resolution>().ValueToDegrade = 1;
        fitts.GetComponent<FieldOfView>().ValueToDegrade = 0;
    }

    public void TurnOffAllDegradation()
    {
        fitts.GetComponent<Contrast>().active = false;
        fitts.GetComponent<ColorHUE>().active = false;
        fitts.GetComponent<Resolution>().active = false;
        fitts.GetComponent<FieldOfView>().active = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SaveState();

        DisplayLiveVariables();
    }

    private void DisplayLiveVariables()
    {
        if (contrastValue != null) contrastValue.text = fitts.GetComponent<Contrast>().ValueToDegrade.ToString();
        if (colorValue != null) colorValue.text = "-" + fitts.GetComponent<ColorHUE>().GetHighestColorShift().ToString();
        if (resolutionValue != null) resolutionValue.text = SteamVR_Camera.sceneResolutionScale * 2880 + "x" + SteamVR_Camera.sceneResolutionScale * 1600;
        if (fovValue != null) fovValue.text = "-" + fitts.GetComponent<FieldOfView>().ValueToDegrade.ToString();
    }
}
