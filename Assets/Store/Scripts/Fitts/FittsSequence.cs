using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    // Class containing information about a Fitts sequence
    public class FittsSequence
    {
        public List<FittsTrial> fittsTrials;

        public SequenceParameter parameter;
        public string timestamp;

        private FittsTrajectory trajectory;

        private bool isInTarget;

        public FittsSequence(SequenceParameter _parameter)
        {
            fittsTrials = new List<FittsTrial>();

            parameter = _parameter;

            trajectory = new FittsTrajectory();

            isInTarget = false;
        }

        public void StartSequence()
        {
            TargetsCreator.CreateTargets(parameter);
            // Timestamp is recorded to analyse the eye data with the sequence
            timestamp = DateTime.Now.ToString("HH:mm:ss:fff");
        }

        public bool AddTriggerEvent(Vector3 pos, FittsTime time, bool trialIsError)
        {
            // Check if first selection because it is not recorded
            if (!TargetsCreator.IsFirstTarget())
                AddTrial(pos, time, trialIsError);

            // Start a new trajectory for the new target because each trial has his trajectory
            trajectory = new FittsTrajectory();

            return parameter.nbOfTarget == fittsTrials.Count;
        }

        public void RecordPosition(List<Collider> targetsColliding, Vector3 pos, float time)
        {
            CheckForTargetReEntry(targetsColliding);
            trajectory.AddCoordinate(pos, time);
        }

        private void CheckForTargetReEntry(List<Collider> targetsColliding)
        {
            if (!isInTarget && (isInTarget = TargetsCreator.IsTouchingSelectTarget(targetsColliding)))
            {
                trajectory.targetReEntry++;
            }
            else
            {
                isInTarget = TargetsCreator.IsTouchingSelectTarget(targetsColliding);
            }
        }

        private void AddTrial(Vector3 pos, FittsTime time, bool trialIsError)
        {
            FittsCoordinate coord = new FittsCoordinate(TargetsCreator.GetLastTargetPosition(), TargetsCreator.GetCurrentTargetPosition(), new Vector2(pos.x, pos.y));
            fittsTrials.Add(new FittsTrial(time, coord, trialIsError, trajectory));
        }
    }

    public struct SequenceParameter
    {
        public SequenceParameter(float _amplitude, float _width, int _nbOfTarget) {
            amplitude = _amplitude;
            width = _width;
            nbOfTarget = _nbOfTarget;
        }

        public float amplitude { get; }
        public float width { get; }
        public int nbOfTarget { get; }
    }
}
