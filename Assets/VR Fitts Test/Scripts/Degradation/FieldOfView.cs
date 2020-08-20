using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class FieldOfView : Degradation
{
    public PostProcessVolume volume;
    private Vignette v;
    public override float ValueToDegrade { get => v.intensity.value;  set { v.intensity.value = value; } }

    protected override void SetDefaultValues()
    {
        volume.profile.TryGetSettings(out v);
        rate.text = "1";
        step.text = "0.005";
    }

    public override void Update()
    {
        // Degrade Graphics each second
        if (active && autoDegradation && Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1 / float.Parse(rate.text);
            ValueToDegrade += float.Parse(step.text);
        }
    }
}
