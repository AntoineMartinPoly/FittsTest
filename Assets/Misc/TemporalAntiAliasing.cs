using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TemporalAntiAliasing : MonoBehaviour
{

    private float jitterSpread = 0.75f;
    private float stationaryBlending = 0.95f;
    private float motionBlending = 0.85f;
    private float sharpness = 0.25f;

    void Start()
    {
        Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.jitterSpread = jitterSpread;
        Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.stationaryBlending = stationaryBlending;
        Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.motionBlending = motionBlending;
        Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.sharpness = sharpness;
    }

    void Update()
    {
        switch (Input.inputString)
        {
            case "f":
                DecJitterSpread();
                DecStationaryBlending();
                DecMotionBlending();
                DecSharpness();
                break;
            case "g":
                IncJitterSpread();
                IncStationaryBlending();
                IncMotionBlending();
                IncSharpness();
                break;
        }
    }
        void IncJitterSpread()
        {
            jitterSpread = jitterSpread + 0.1f > 1f ? 1f : jitterSpread += 0.1f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.jitterSpread = jitterSpread;
        }

        void IncStationaryBlending()
        {
            stationaryBlending = stationaryBlending + 0.1f > 1f ? 1f : stationaryBlending += 0.1f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.stationaryBlending = stationaryBlending;
        }

        void IncMotionBlending()
        {
            motionBlending = motionBlending + 0.1f > 1f ? 1f : motionBlending += 0.1f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.motionBlending = motionBlending;
        }

        void IncSharpness()
        {
            sharpness = sharpness + 0.3f > 3f ? 3f : sharpness += 0.3f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.sharpness = sharpness;
        }

        void DecJitterSpread()
        {
            jitterSpread = jitterSpread - 0.1f < 0.1f ? 0.1f : jitterSpread -= 0.1f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.jitterSpread = jitterSpread;
        }

        void DecStationaryBlending()
        {
            stationaryBlending = stationaryBlending - 0.1f < 0 ? 0 : stationaryBlending -= 0.1f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.stationaryBlending = stationaryBlending;
        }

        void DecMotionBlending()
        {
            motionBlending = motionBlending - 0.1f < 0 ? 0 : motionBlending -= 0.1f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.motionBlending = motionBlending;
        }

        void DecSharpness()
        {
            sharpness = sharpness - 0.3f < 0 ? 0 : sharpness -= 0.3f;
            Camera.main.GetComponent<PostProcessLayer>().temporalAntialiasing.sharpness = sharpness;
        }
    }
