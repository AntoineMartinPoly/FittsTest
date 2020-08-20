using Valve.VR;

public class Resolution : Degradation
{
    public override float ValueToDegrade { get => SteamVR_Camera.sceneResolutionScale; set { SteamVR_Camera.sceneResolutionScale = value; } }

    protected override void SetDefaultValues()
    {
        rate.text = "1";
        step.text = "0.005";
    }
}
