using UnityEngine;

namespace Assets.Scripts
{
    // Class containing information about a fitts trial
    public class FittsTrial
    {
        public FittsTime time;
        public FittsCoordinate coord;
        public FittsTrajectory trajectory;

        public bool isError;
        public double effectiveAmplitude;
        public double dx;

        public FittsTrial(FittsTime _time, FittsCoordinate _coord , bool isTouchingTarget, FittsTrajectory _trajectory)
        {
            time = _time;
            coord = _coord;
            trajectory = _trajectory;
            isError = !isTouchingTarget;
        }
    }

    public struct FittsTime {
        public FittsTime(float _pointingTime, float _selectionTime)
        {
            pointingTime = _pointingTime;
            selectionTime = _selectionTime;
            movementTime = pointingTime + selectionTime;
        }

        public float pointingTime { get; }
        public float selectionTime { get;  }
        public float movementTime { get; }
    }

    public struct FittsCoordinate
    {
        public FittsCoordinate(Vector2 _from, Vector2 _to, Vector2 _select)
        {
            select = _select;
            from = _from;
            to = _to;
        }

        public Vector2 from { get; }
        public Vector2 to { get; }
        public Vector2 select { get; }
    }
}
