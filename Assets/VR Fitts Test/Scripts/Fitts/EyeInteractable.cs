using Tobii.G2OM;
using UnityEngine;

// Script attach to Fitts target to record eye information
// This class record the number of second the target was looked at and the number of time it was focus
public class EyeInteractable : MonoBehaviour, IGazeFocusable
{
    public float gazeTime;
    public int nbOfFocus = 0;
    private float startingGazeTime;

    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            nbOfFocus++;
            startingGazeTime = Time.realtimeSinceStartup;
        }
        else
        {
            gazeTime += Time.realtimeSinceStartup - startingGazeTime;
        }
    }
}
