using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    // This class calculates the MV, ME, MO, TAC, MDC, ODC after the test with the recorded trajectory
    // To learn more about those variables see http://www.yorku.ca/mack/CHI01.htm
    public class FittsMovement
    {
        List<double> deltaY;

        public double movementVariable;
        public double movementError;
        public double movementOffset;

        public int taskAxisCrossing;
        public int movementDirectionChange;
        public int orthogonalDirectionChange;

        // Threshold in meters used to measure MDC and ODC without which to many MDC/ODC are recorded
        private readonly double threshold;

        public FittsMovement(FittsCoordinate coord, FittsTrajectory trajectory)
        {
            threshold = 0.01f;

            CreateDeltaYList(coord, trajectory);

            // MO
            movementOffset = deltaY.Sum() / trajectory.y.Count;
            // ME
            movementError = deltaY.Sum(y => Math.Abs(y)) / trajectory.y.Count;
            // MV
            movementVariable = Math.Sqrt( deltaY.Sum(y => Math.Pow(y - movementOffset,2)) / (trajectory.x.Count - 1) );

            // TAC
            CalculateTaskAxisCrossing();
            // MDC
            CalculateMovementDirectionChange();
            // ODC
            CalculateOrthogonalDirectionChange(coord, trajectory);
        }

        private void CreateDeltaYList(FittsCoordinate coord, FittsTrajectory trajectory)
        {
            // Contains the delta y from the controller trajectory and the projected line between from and select
            deltaY = new List<double>();

            // Ax + By + C = 0
            float a = (coord.select.y - coord.from.y) / (coord.select.x - coord.from.x);
            float b = -1;
            float c = coord.select.y - coord.select.x * a;

            // Fetch x and y coord of trajectory
            var trajectoryCoords = trajectory.x.Zip(trajectory.y, (_x, _y) => new { x = _x, y = _y });

            // Find each delta y for each trajectory coordinate
            foreach (var trajCoord in trajectoryCoords)
            {
                deltaY.Add((a * trajCoord.x + b * trajCoord.y + c) / (Math.Sqrt(a * a + b * b)));
            }
        }

        private void CalculateTaskAxisCrossing()
        {
            bool distanceIsPositive = deltaY[0] >= 0;
            foreach(double y in deltaY)
            {
                if (distanceIsPositive && y < 0 || !distanceIsPositive && y >= 0)
                    taskAxisCrossing++;
                distanceIsPositive = y >= 0;
            }
        }

        private void CalculateMovementDirectionChange()
        {
            List<double> totalYDifference = new List<double>();

            bool yIsGrowing = deltaY[1] >= deltaY[0];
            
            double lastY = deltaY[1];

            for (int i = 2; i < deltaY.Count; i++)
            {
                // Check to see if the trajectory continues in the same direction
                if (yIsGrowing == deltaY[i] >= lastY)
                {
                    lastY = deltaY[i];
                    // We clear the distance covered in new direction as not to add past difference to actual one
                    totalYDifference.Clear();
                    continue;
                }
                // A new direction is chosen so we add up the distance covered in the new direction
                // to record a MDC only if it reach a certain threshold
                totalYDifference.Add(deltaY[i] < lastY ? lastY - deltaY[i] : deltaY[i] - lastY);
                // The threshold is reached
                if (totalYDifference.Sum() > threshold)
                {
                    yIsGrowing = deltaY[i] >= lastY;
                    totalYDifference.Clear();
                    movementDirectionChange++;
                }
                lastY = deltaY[i];
            }
        }

        private void CalculateOrthogonalDirectionChange(FittsCoordinate coord, FittsTrajectory trajectory)
        {
            List<double> totalDistanceDifference = new List<double>();

            Vector2 firstPoint = new Vector2(trajectory.x[0], trajectory.y[0]);
            double lastDistance = Vector2.Distance(coord.select, firstPoint);

            bool distanceIsShorter = true;

            for (int i = 0; i < trajectory.x.Count; i++)
            {
                double currentDistance = Vector2.Distance(coord.select, new Vector2(trajectory.x[i], trajectory.y[i]));
                // Check to see if the trajectory continues in the same direction
                if (distanceIsShorter == currentDistance <= lastDistance)
                {
                    lastDistance = currentDistance;
                    // We clear the distance covered in new direction as not to add past difference to actual one
                    totalDistanceDifference.Clear();
                    continue;
                }
                // A new direction is chosen so we add up the distance covered in the new direction
                // to record a MDC only if it reach a certain threshold
                totalDistanceDifference.Add(currentDistance > lastDistance ? currentDistance - lastDistance : lastDistance - currentDistance);
                // The threshold is reached
                if (totalDistanceDifference.Sum() > threshold)
                {
                    distanceIsShorter = currentDistance <= lastDistance;
                    totalDistanceDifference.Clear();
                    orthogonalDirectionChange++;
                }
                lastDistance = currentDistance;
            }
        }
    }
}
