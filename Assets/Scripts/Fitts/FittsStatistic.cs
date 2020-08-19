using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    // This class calculates the statistics about the fitts test
    // These statistics are exported afterward in csv format and shown live in the main menu
    // To learn more about which variables are exported : https://www.yorku.ca/mack/FittsLawSoftware/doc/FittsTask.html
    public static class FittsStatistic
    {
        public static List<SequenceStatistic> sequencesStats;
        public static double movementTimeMean;
        public static double errorMean;
        public static double throughputMean;

        public static void AddFittsSequence(FittsSequence sequence)
        {
            if (sequencesStats == null)
                sequencesStats = new List<SequenceStatistic>();

            sequencesStats.Add(new SequenceStatistic(sequence));

            movementTimeMean = sequencesStats.Sum(x => x.timeMean.movementTime) / sequencesStats.Count;
            errorMean = sequencesStats.Sum(x => x.errorMean) / sequencesStats.Count;
            throughputMean = sequencesStats.Sum(x => x.throughput) / sequencesStats.Count;
        }

        public static void CreateNewStatistic()
        {
            sequencesStats = new List<SequenceStatistic>();
            movementTimeMean = 0;
            errorMean = 0;
            throughputMean = 0;
        }
    }

    public struct SequenceStatistic
    {
        public SequenceStatistic(FittsSequence _sequence)
        {
            trialsStats = new List<TrialStatistic>();
            sequence = _sequence;
            foreach (FittsTrial trial in sequence.fittsTrials)
            {
                trialsStats.Add(new TrialStatistic(trial));
            }
            // We
            effectiveWidth = 4.133 * CalculateStandardDeviation(new List<double>(trialsStats.Select(x => x.dx)));
            // Ae (mean)
            effectiveAmplitudeMean = trialsStats.Sum(x => x.effectiveAmplitude) / trialsStats.Count;
            // IDe
            effectiveIndexDifficulty = Math.Log((effectiveAmplitudeMean / effectiveWidth) + 1, 2);
            // TP
            throughput = effectiveIndexDifficulty / (sequence.fittsTrials.Sum(x => x.time.movementTime) / sequence.fittsTrials.Count);
            // MT
            timeMean = new FittsTime(sequence.fittsTrials.Sum(x => x.time.pointingTime) / sequence.fittsTrials.Count, sequence.fittsTrials.Sum(x => x.time.selectionTime) / sequence.fittsTrials.Count);
            // ER(%)
            errorMean = (double)sequence.fittsTrials.Where(x => x.isError).Count() / (double)sequence.fittsTrials.Count;
            errorMean *= 100;
            // Eye Data
            eyeData = TargetsCreator.GetEyeData();
        }

        private static double CalculateStandardDeviation(List<double> values)
        {
            double standardDeviation = 0;
            if (values.Any())
            {
                double avg = values.Average();
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return standardDeviation;
        }
        public FittsSequence sequence { get; }
        public List<TrialStatistic> trialsStats { get; }
        public double effectiveAmplitudeMean { get; }
        public double effectiveWidth { get; }
        public double effectiveIndexDifficulty { get; }
        public double throughput { get; }
        public FittsTime timeMean { get; }
        public double errorMean { get; }
        public EyeInteractable[] eyeData {get;}
    }
    public struct TrialStatistic
    {
        public TrialStatistic(FittsTrial _trial)
        {
            trial = _trial;
            // dx
            double a = Vector2.Distance(trial.coord.from, trial.coord.to);
            double b = Vector2.Distance(trial.coord.to, trial.coord.select);
            double c = Vector2.Distance(trial.coord.from, trial.coord.select);
            dx = (c * c - b * b - a * a) / (2.0 * a);
            // Ae
            effectiveAmplitude = a + dx;
            // Movement variability, movement error and movement offset
            movement = new FittsMovement(trial.coord, trial.trajectory);
        }
        public FittsTrial trial { get; }
        public double effectiveAmplitude { get; }
        public double dx { get; }
        public FittsMovement movement { get; }
    }
}

