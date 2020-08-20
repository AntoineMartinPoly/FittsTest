using Assets.Scripts;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Class attached to the Live Variables section of the menu
public class LiveVariables : MonoBehaviour
{
    public Text sequenceText;
    public Text trialText;
    public GameObject controller;
    void Update()
    {
        if (FittsStatistic.sequencesStats != null)
        {
            sequenceText.text = "";
            foreach (SequenceStatistic stat in FittsStatistic.sequencesStats)
            {
                sequenceText.text += "Amplitude : " + stat.sequence.parameter.amplitude +
                                 "    Width : " + stat.sequence.parameter.width +
                                 "    Error : " + stat.errorMean +
                                 "    TP : " + stat.throughput + "\n";
            }
        }
        if (controller.GetComponent<VRController>().ft != null && controller.GetComponent<VRController>().ft.currentFittsSequence.fittsTrials.Count > 0)
        {
            FittsSequence currentSequence = controller.GetComponent<VRController>().ft.currentFittsSequence;
            trialText.text = "Error : " + currentSequence.fittsTrials.Last().isError +
                    "  time : " + currentSequence.fittsTrials.Last().time.movementTime +
                    "  coord : " + currentSequence.fittsTrials.Last().coord.select.x + " " + currentSequence.fittsTrials.Last().coord.select.y +
                    "  A : " + currentSequence.parameter.amplitude + " W : " + currentSequence.parameter.width;
        }
    }
}
