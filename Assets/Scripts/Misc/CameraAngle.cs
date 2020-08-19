using UnityEngine;

public class CameraAngle : MonoBehaviour
{
    private void Start()
    {
        // gameObject.transform.position = new Vector3(-0.1f, 2.1f, -2.4f);
    }
    void Update()
    {
        switch (Input.inputString)
        {
            case "4":
                gameObject.transform.Rotate(0,-30,0);
                break;
            case "6":
                gameObject.transform.Rotate(0, 30, 0);
                break;
            case "r":
                Valve.VR.OpenVR.System.ResetSeatedZeroPose();
                Valve.VR.OpenVR.Compositor.SetTrackingSpace(
                Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
                break;
        }
    }
}
