using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// This class manages controller event and send them to a FittsTest
public class VRController : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;
    public SteamVR_Action_Boolean trigger;
    public FittsTest ft;

    private Material red;
    private Material green;

    private float lastTriggerTime;
    private float pointingTime;
    private float selectionTime;

    private List<Collider> colliders;

    public void AddNewFittsTest(FittsTestParameter fittsParameter)
    {
        ft = new FittsTest(fittsParameter);
    }

    private void Start()
    {
        red = Resources.Load("Materials/red") as Material;
        green = Resources.Load("Materials/green") as Material;
        lastTriggerTime = 0;
        colliders = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vibrate controller when it touches the detection plane
        if (other.gameObject.tag == "detectionPlane")
        {
            hapticAction.Execute(0, 0.1f, 75, 75, SteamVR_Input_Sources.RightHand);
            hapticAction.Execute(0, 0.1f, 75, 75, SteamVR_Input_Sources.LeftHand);
        }
    }

    private void Update()
    {
        RecordTrajectory();
        CheckForControllerTriggerStateDown();
        CheckForControllerTriggerStateUp();
    }

    private void RecordTrajectory()
    {
        if (ft != null)
        {
            // A List containing all Collider touching the OverlapSphere which is the red/green sphere in front of the controller
            colliders = new List<Collider>(Physics.OverlapSphere(transform.position, 0.025f));
            ft.RecordPosition(colliders, transform.position, Time.realtimeSinceStartup - lastTriggerTime);
        }
    }

    private void CheckForControllerTriggerStateDown()
    {
        // Trigger pressed
        if (trigger.GetStateDown(SteamVR_Input_Sources.RightHand) || trigger.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            gameObject.GetComponent<Renderer>().material = green;
            pointingTime = Time.realtimeSinceStartup - lastTriggerTime;
            selectionTime = Time.realtimeSinceStartup - pointingTime - lastTriggerTime;
            RecordTrigger();
        }
    }

    private void CheckForControllerTriggerStateUp()
    {
        // Trigger released
        if (trigger.GetStateUp(SteamVR_Input_Sources.RightHand) || trigger.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            gameObject.GetComponent<Renderer>().material = red;
        }
    }

    private void RecordTrigger()
    {
        lastTriggerTime = Time.realtimeSinceStartup;

        if (colliders.Find(x => x.gameObject.CompareTag("detectionPlane")) != null)
            ft.RecordTriggerEventAndSetNewTarget(transform.position, new FittsTime(pointingTime, selectionTime), TargetsCreator.IsTouchingSelectTarget(colliders));
    }
}
