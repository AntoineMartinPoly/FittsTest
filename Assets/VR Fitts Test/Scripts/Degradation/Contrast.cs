using UnityEngine.Rendering.PostProcessing;

public class Contrast : Degradation
{
    public PostProcessVolume volume;
    private ColorGrading cg;
    public override float ValueToDegrade { get => cg.contrast.value; set { cg.contrast.value = value; } }

    protected override void SetDefaultValues()
    {
        volume.profile.TryGetSettings(out cg);
        rate.text = "1";
        step.text = "0.5";
    }
}
