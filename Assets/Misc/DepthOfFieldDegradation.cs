using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldDegradation : MonoBehaviour
{
    public PostProcessVolume volume;
    private ColorGrading cg;
    private int nextUpdate = 1;
    private DepthOfField dof;

    private void Start()
    {
        volume.profile.TryGetSettings(out dof);
        volume.profile.TryGetSettings(out cg);
        dof.active = true;
        dof.focusDistance.value = 0.3f;
        dof.aperture.value = 32;
        dof.focalLength.value = 30;
        dof.kernelSize.value = KernelSize.VeryLarge;
    }

    private void Update()
    {
        // Degrade Graphics each second
        //if (Time.time >= nextUpdate)
        //{
        //    Debug.Log(Time.time + ">=" + nextUpdate);
        //    nextUpdate = Mathf.FloorToInt(Time.time) + 1;
        //    ReduceApertureSlowly();
        //}

        // Degrade Graphics by keyboard input
        switch (Input.inputString)
        {
            case "f":
                //dof.focusDistance.value -= 0.1f;
                dof.aperture.value -= 1;
                //dof.focalLength.value -= 10;
                Debug.Log("downgraded graphics : " + dof.aperture.value);
                break;
            case "g":
                //dof.focusDistance.value += 0.1f;
                dof.aperture.value += 1;
                //dof.focalLength.value += 10;
                Debug.Log("upgraded graphics : " + dof.aperture.value);
                break;
        }
    }

    void ReduceApertureSlowly()
    {
        dof.aperture.value -= 0.2f;
    }
}
