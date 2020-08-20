using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This class is in charge of creating and managing the target used in the Fitts test
public class TargetsCreator : MonoBehaviour
{
    private const string YELLOW_MATERIAL = "Materials/yellow";
    private const string DARK_YELLOW_MATERIAL = "Materials/darkYellow";
    private const string DARK_BLUE_MATERIAL = "Materials/darkBlue";
    private const string BLENDER_TARGET_PREFAB = "BlenderTarget";
    private const string DETECTION_PLANE_PREFAB = "detectionPlane";

    private static GameObject[] targets;
    private static GameObject detectionPlane = null;
    private static GameObject highlightTarget;

    private static Vector3 initialCameraPosition;

    private static int targetIndexToSelect;
    private static int lastTargetIndexToSelect;

    public static float distanceToTarget { get; set; }

    public static void CreateTargets(SequenceParameter parameter)
    {
        targets = new GameObject[parameter.nbOfTarget];
        CreateDetectionPlane();
        for (int i = 0; i < parameter.nbOfTarget; i++)
        {
            targets[i] = CreateTarget(parameter, i);
        }
        SetInitialTarget();
    }

    public static void SetNextTargetIndexAndColor()
    {
        if (targets != null)
        {
            SetTargetMaterialBlue(targetIndexToSelect);
            lastTargetIndexToSelect = targetIndexToSelect;
            targetIndexToSelect = (targetIndexToSelect + (targets.Length / 2) + 1) % targets.Length;
            SetTargetMaterialYellow(targetIndexToSelect);
        }
    }

    public static bool IsFirstTarget()
    {
        return targetIndexToSelect == 0 && lastTargetIndexToSelect == 0;
    }

    public static Vector2 GetLastTargetPosition()
    {
        return new Vector2(targets[lastTargetIndexToSelect].transform.position.x, targets[lastTargetIndexToSelect].transform.position.y);
    }

    public static Vector2 GetCurrentTargetPosition()
    {
        return new Vector2(targets[targetIndexToSelect].transform.position.x, targets[targetIndexToSelect].transform.position.y);
    }

    public static void DestroyTargets()
    {
        if (targets != null)
        {
            foreach(GameObject target in targets)
            {
                Destroy(target);
            }
            targets = null;
        }
        if (highlightTarget != null)
            Destroy(highlightTarget);
        if (detectionPlane != null)
            Destroy(detectionPlane);
    }

    public static bool IsTouchingSelectTarget(List<Collider> targetsColliding)
    {
        return targets != null && targetsColliding.Select(x => x.gameObject).Contains(targets[targetIndexToSelect]);
    }

    public static EyeInteractable[] GetEyeData()
    {
        return targets.Select(x => x.GetComponent<EyeInteractable>()).ToArray();
    }

    public static void StartNewTest(float _distanceToTarget)
    {
        // Keep the initial position of the camera when the test begin so the targets are not created at different distance from the user throughout the test
        initialCameraPosition = Camera.main.transform.position;
        distanceToTarget = _distanceToTarget;
        DestroyTargets();
    }

    private static void SetTargetMaterialYellow(int index)
    {
        targets[index].gameObject.GetComponent<Renderer>().material = Resources.Load(YELLOW_MATERIAL) as Material;
        CreateHighlightTarget();
        targets[index].gameObject.transform.position += Vector3.back * 0.0001f;
    }

    private static void SetTargetMaterialBlue(int index)
    {
        targets[index].gameObject.GetComponent<Renderer>().material = Resources.Load(DARK_BLUE_MATERIAL) as Material;
        targets[index].gameObject.transform.position += Vector3.forward * 0.0001f;
    }

    private static GameObject CreateTarget(SequenceParameter parameter, int circleIndex)
    {
        GameObject target = Instantiate(Resources.Load(BLENDER_TARGET_PREFAB) as GameObject, initialCameraPosition + Vector3.forward * distanceToTarget, Quaternion.identity);

        target.transform.position += FindCircleCoordinate(circleIndex, parameter.amplitude, parameter.nbOfTarget);

        // Apply width chosen in the fitts test
        target.transform.localScale *= parameter.width;

        return target;
    }

    private static void SetInitialTarget()
    {
        targetIndexToSelect = 0;
        lastTargetIndexToSelect = 0;

        SetTargetMaterialYellow(targetIndexToSelect);
    }

    private static void CreateHighlightTarget()
    {
        // Create a target a bit larger and a bit darker around the yellow target
        if (highlightTarget != null) Destroy(highlightTarget);
        highlightTarget = Instantiate(Resources.Load(BLENDER_TARGET_PREFAB) as GameObject, targets[targetIndexToSelect].gameObject.transform.position, Quaternion.identity);
        highlightTarget.transform.localScale = targets[targetIndexToSelect].gameObject.transform.localScale * 1.05f;
        highlightTarget.transform.position += Vector3.back * 0.00005f;
        highlightTarget.GetComponent<Renderer>().material = Resources.Load(DARK_YELLOW_MATERIAL) as Material;
    }

    private static Vector3 FindCircleCoordinate(double inclination, float amplitude, int nbOfTarget)
    {
        double piAngle = -(inclination / nbOfTarget) * Math.PI * 2;
        return new Vector3((float)Math.Cos(piAngle) * amplitude, (float)Math.Sin(piAngle) * amplitude, 0);
    }

    private static void CreateDetectionPlane()
    {
        if (detectionPlane != null) Destroy(detectionPlane);
        // The detection plane must be a little further away then the target to be able to record the eye focus on the target
        detectionPlane = Instantiate(Resources.Load(DETECTION_PLANE_PREFAB) as GameObject, initialCameraPosition + Vector3.forward * distanceToTarget * 1.005f, Quaternion.identity);
    }
}
