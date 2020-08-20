
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Fitts;

// This script is following the same logic of Scott MacKenzie's Fitts Law Software 
// (documentation found here http://www.yorku.ca/mack/FittsLawSoftware/)
// It is a serial 3D simulation of the 2D test of Fitts Law Software
//
// Class containing information about a Fitts test
public class FittsTest
{
    public FittsSequence currentFittsSequence;

    private List<FittsSequence> fittsSequences;
    private HashSet<int> fittsIndex;
    private FittsTestParameter testParameter;

    public FittsTest(FittsTestParameter _testParameter)
    {
        testParameter = _testParameter;

        CreateRandomSequenceIndexSet();
        CreateFittsSequences();

        currentFittsSequence = fittsSequences[fittsIndex.Pop()];
        currentFittsSequence.StartSequence();

    }

    public void RecordTriggerEventAndSetNewTarget(Vector3 pos, FittsTime time, bool trialIsError)
    {
        // Return true when the last selection of the sequence have been made
        if (currentFittsSequence.AddTriggerEvent(pos, time, trialIsError))
        {
            // The sequence is added to statistic
            FittsStatistic.AddFittsSequence(currentFittsSequence);
            TargetsCreator.DestroyTargets();
            if (fittsIndex.Count != 0)
            {
                // There is still fitts sequence to be done
                // A new sequence is started
                currentFittsSequence = fittsSequences[fittsIndex.Pop()];
                currentFittsSequence.StartSequence();
            } else
            {
                // All sequences are done
                FittsExport.ExportFittsToCSV();
                EyeExport.ExportEyeToCSV();
            }
        } else
        {
            TargetsCreator.SetNextTargetIndexAndColor();
        }
    }

    public void RecordPosition(List<Collider> targetsColliding, Vector3 pos, float time)
    {
        currentFittsSequence.RecordPosition(targetsColliding, pos, time);
    }

    private void CreateRandomSequenceIndexSet()
    {
        fittsIndex = new HashSet<int>();

        // Add random number index to create a random combination of single fitts test 
        while (fittsIndex.Count != testParameter.amplitude.Count * testParameter.width.Count * testParameter.nbOfTarget.Count)
            fittsIndex.Add(Random.Range(0, testParameter.amplitude.Count * testParameter.width.Count * testParameter.nbOfTarget.Count));
    }

    private void CreateFittsSequences()
    {
        fittsSequences = new List<FittsSequence>();
        // Create a list with all combination of amplitude, width and number of target possible
        var test = from a in testParameter.amplitude
                   from w in testParameter.width
                   from nb in testParameter.nbOfTarget
                   select new { a, w, nb };
        foreach (var t in test)
        {
            fittsSequences.Add(new FittsSequence(new SequenceParameter(t.a / 2, t.w, t.nb)));
        }
    }
}

static class Extensions
{
    // This Extension is added to be able to use a Stack Pop() equivalent with the Hashset
    public static T Pop<T>(this ICollection<T> objects)
    {
        T o = objects.FirstOrDefault();
        if (o != null)
        {
            objects.Remove(o);
        }
        return o;
    }
}

public struct FittsTestParameter
{
    public FittsTestParameter(List<float> _amplitude, List<float> _width, List<int> _nbOfTarget)
    {
        amplitude = _amplitude;
        width = _width;
        nbOfTarget = _nbOfTarget;
    }
    public List<float> amplitude { get; }
    public List<float> width { get; }
    public List<int> nbOfTarget { get; }
}