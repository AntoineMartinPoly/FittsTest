using Assets.Scripts;
using Assets.Scripts.Fitts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Class attached to the Test Parameter section of the menu
public class TestParameters : MonoBehaviour
{
    public InputField amplitude;
    public InputField width;
    public InputField nbOfTarget;
    public InputField distanceToTarget;
    public InputField participantID;

    public Toggle exportFittsData;
    public Toggle exportEyeData;

    public GameObject controller;
    public GameObject fitts;

    public Toggle contrast;
    public Toggle contrastAuto;
    public Toggle color;
    public Toggle colorAuto;
    public Toggle resolution;
    public Toggle resolutionAuto;
    public Toggle fov;
    public Toggle fovAuto;

    private void Awake()
    {
        SetDefaultValues();
        AddListeners();
    }

    public void StartTest()
    {
        TargetsCreator.StartNewTest(float.Parse(distanceToTarget.text));

        // Create a new Fitts test with the parameter from the menu
        controller.GetComponent<VRController>().AddNewFittsTest(GetFittsTestParameter());

        // Set up new environnement and stats for new Fitts Test
        GameObject.Find("Fitts").GetComponent<EyeTracking>().CreateNewEyesStatistic();
        FittsStatistic.CreateNewStatistic();

        SetDregradationActive();
    }

    public void ClearTest()
    {
        controller.GetComponent<VRController>().ft = null;
        TargetsCreator.DestroyTargets();
    }

    public void SetDregradationActive()
    {
        fitts.GetComponent<Contrast>().SetActive(contrast.isOn, contrastAuto.isOn);
        fitts.GetComponent<ColorHUE>().SetActive(color.isOn, colorAuto.isOn);
        fitts.GetComponent<Resolution>().SetActive(resolution.isOn, resolutionAuto.isOn);
        fitts.GetComponent<FieldOfView>().SetActive(fov.isOn, fovAuto.isOn);
    }

    private void SetDefaultValues()
    {
        amplitude.text = "0.3, 0.4";
        width.text = "0.2, 0.4";
        nbOfTarget.text = "9";
        distanceToTarget.text = "0.5";
        participantID.text = "P0";
    }

    private void AddListeners()
    {
        FittsExport.participantID = participantID.text;
        FittsExport.exportIsOn = exportFittsData.isOn;
        EyeExport.exportIsOn = exportEyeData.isOn;
        participantID.onValueChanged.AddListener((newID) => { FittsExport.participantID = newID; });
        exportFittsData.onValueChanged.AddListener((exportFitts) => { FittsExport.exportIsOn = exportFitts; });
        exportEyeData.onValueChanged.AddListener((exportEye) => { FittsExport.exportIsOn = exportEye; });
    }

    private FittsTestParameter GetFittsTestParameter()
    {
        return new FittsTestParameter(
                amplitude.text.Split(',').Select(Convert.ToSingle).ToList(),
                width.text.Split(',').Select(Convert.ToSingle).ToList(),
                nbOfTarget.text.Split(',').Select(x => Convert.ToInt32(x)).ToList()
            );
    }
}
