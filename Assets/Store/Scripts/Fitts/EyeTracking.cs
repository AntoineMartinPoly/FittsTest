using UnityEngine;
using ViveSR.anipal.Eye;
using System;
using System.Collections.Generic;

// Class that record eye data during the whole test using SRanipal sdk
public class EyeTracking : MonoBehaviour
{
    public List<EyeExportStatistic> eyeStatistic = new List<EyeExportStatistic>();
    private EyeData eye = new EyeData();

    private void Update()
    {
        SRanipal_Eye_API.GetEyeData(ref eye);
        eyeStatistic.Add(new EyeExportStatistic(eye));
    }

    public void CreateNewEyesStatistic()
    {
        eyeStatistic = new List<EyeExportStatistic>();
    }

    public struct EyeExportStatistic
    {
        public EyeExportStatistic(EyeData eye)
        {
            timestamp = DateTime.Now.ToString("HH:mm:ss:fff");
            leftEye = eye.verbose_data.left;
            rightEye = eye.verbose_data.right;
            combinedEye = eye.verbose_data.combined;
        }
        public string timestamp { get; }
        public SingleEyeData leftEye { get; }
        public SingleEyeData rightEye { get; }
        public CombinedEyeData combinedEye { get; }
    }
}
