using Assets.Scripts.Fitts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ColorHUE : Degradation
{
    public Dropdown nbOfColor;
    public override float ValueToDegrade { get => float.Parse(step.text); set { DegradeColors(); } }

    public PostProcessVolume volume;

    private ColorGrading cg;
    private List<int> colorsIndex;
    private Keyframe[] tempkf;
    private int numberOfKeyframe;

    protected override void SetDefaultValues()
    {
        volume.profile.TryGetSettings(out cg);
        rate.text = "1";
        step.text = "0.0005";
        numberOfKeyframe = 1000;
        CreateKeyframeArray();
        CreateColorsIndex();
    }

    public List<Vector2> GetKeyframes()
    {
        List<Vector2> keyframes = new List<Vector2>();
        foreach(Keyframe key in cg.hueVsHueCurve.value.curve.keys)
        {
            keyframes.Add(new Vector2(key.time, key.value));
        }
        return keyframes;
    }

    public void ResetKeyFrames()
    {
        for (int i = 0; i < tempkf.Length; i++)
        {
            tempkf[i].value = 0.5f;
        }
        cg.hueVsHueCurve.value.curve.keys = tempkf;
    }

    public override void IncrementDegradation()
    {
        DegradeColors();
    }

    public override void DecrementDegradation()
    {
        UpgradeColors();
    }

    public float GetHighestColorShift()
    {
        float highestShift = 0;
        foreach(Keyframe keyframe in cg.hueVsHueCurve.value.curve.keys)
        {
            if (highestShift < Math.Abs(keyframe.value - 0.5f))
                highestShift = Math.Abs(keyframe.value - 0.5f);
        }
        return highestShift;
    }

    private void DestroyKeyframes()
    {
        cg.hueVsHueCurve.value.curve.keys = null;
        colorsIndex = null;
    }

    private void CreateKeyframeArray()
    {
        DestroyKeyframes();
        // Create a list (kf) of Keyframes with index representing the x value of the Hue vs Hue Grading Curves
        for (int i = 0; i < numberOfKeyframe; i++)
        {
            cg.hueVsHueCurve.value.curve.AddKey(new Keyframe(i * 0.001f, 0.5f, 1f / 0f, 1f / 0f));
        }
    }
    
    private void CreateColorsIndex()
    {
        int step = numberOfKeyframe / int.Parse(nbOfColor.options[nbOfColor.value].text);
        int index = 0;
        colorsIndex = new List<int>();
        while (colorsIndex.Count != int.Parse(nbOfColor.options[nbOfColor.value].text))
        {
            colorsIndex.Add(index);
            index += step;
        }
    }

    private void DegradeColors()
    {
        foreach(int colorIndex in colorsIndex)
        {
            DegradeColor(colorIndex);
        }
    }

    private void DegradeColor(int colorIndex)
    {
        DegradeKeyframe(colorIndex, numberOfKeyframe / int.Parse(nbOfColor.options[nbOfColor.value].text) / 2);
    }

    private void DegradeKeyframe(int colorIndex, int shift)
    {
        // You can't change keyframe one by one
        // You need to assign an array of keyframe so we make a copy
        tempkf = cg.hueVsHueCurve.value.curve.keys;

        int indexToDegrade;

        // Keyframe below the targeted color
        for (int i = colorIndex - shift; i <= colorIndex; i++)
        {
            indexToDegrade = i <= 0 ? numberOfKeyframe - 1 + i : i;
            tempkf[indexToDegrade] = IncrementKeyframe(tempkf[indexToDegrade], 0.5f + (1f / (float)numberOfKeyframe * Math.Abs(colorIndex - (i <= 0 ? i - 1 : i))));
        }

        // Keyframe above the targeted color
        for (int i = colorIndex; i <= colorIndex + shift; i++)
        {
            indexToDegrade = i > numberOfKeyframe ? i - numberOfKeyframe : i;
            tempkf[indexToDegrade] = DecrementKeyframe(tempkf[indexToDegrade], 0.5f - (1f / (float)numberOfKeyframe * Math.Abs(colorIndex - i)));
        }

        cg.hueVsHueCurve.value.curve.keys = tempkf;
    }

    private void UpgradeColors()
    {
        foreach (int colorIndex in colorsIndex)
        {
            UpgradeColor(colorIndex);
        }
    }

    private void UpgradeColor(int colorIndex)
    {
        UpgradeKeyframe(colorIndex, numberOfKeyframe / int.Parse(nbOfColor.options[nbOfColor.value].text) / 2);
    }

    private void UpgradeKeyframe(int colorIndex, int shift)
    {
        // Keyframes can't be change one by one
        // A whole array need to be assigned
        tempkf = cg.hueVsHueCurve.value.curve.keys;

        int indexToDegrade;

        // Keyframe below the targeted color
        for (int i = colorIndex - shift; i <= colorIndex; i++)
        {
            indexToDegrade = i <= 0 ? numberOfKeyframe - 1 + i : i;
            tempkf[indexToDegrade] = DecrementKeyframe(tempkf[indexToDegrade], 0.5f);
        }

        // Keyframe above the targeted color
        for (int i = colorIndex; i <= colorIndex + shift; i++)
        {
            indexToDegrade = i > numberOfKeyframe ? i - numberOfKeyframe : i;
            tempkf[indexToDegrade] = IncrementKeyframe(tempkf[indexToDegrade], 0.5f);
        }

        cg.hueVsHueCurve.value.curve.keys = tempkf;
    }

    private Keyframe IncrementKeyframe(Keyframe key, float max)
    {
        if (FloatComparator.FirstIsLessThanSecond(key.value, max))
        {
            key.value += float.Parse(step.text);
            return key;
        }
        return key;
    }

    private Keyframe DecrementKeyframe(Keyframe key, float min)
    {
        if (FloatComparator.FirstIsMoreThanSecond(key.value, min))
        {
            key.value -= float.Parse(step.text);
            return key;
        }
        return key;
    }
}
