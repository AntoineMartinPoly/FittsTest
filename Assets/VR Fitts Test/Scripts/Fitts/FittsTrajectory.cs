using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    // this class contains the movement of the controller with timestamp before selecting a target
    public class FittsTrajectory
    {
        public List<float> x;
        public List<float> y;
        public List<float> time;
        public int targetReEntry;

        public FittsTrajectory()
        {
            x = new List<float>();
            y = new List<float>();
            time = new List<float>();
            targetReEntry = 0;
        }

        public void AddCoordinate(Vector3 _pos, float _time)
        {
            x.Add(_pos.x);
            y.Add(_pos.y);
            time.Add(_time);
        }
    }
}
