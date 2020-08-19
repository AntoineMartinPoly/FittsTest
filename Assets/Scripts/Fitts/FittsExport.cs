using Assets.Scripts.Fitts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    // This class is used to export in a csv
    static class FittsExport
    {
        public static bool exportIsOn;
        public static string participantID { get; set; }
        public static string path = @"C:\Users\antoi\OneDrive - polymtl.ca\stage\Unity\Fitts Test Data\test unity\last_test";

        private static StringBuilder sbTrajectory;
        private static StringBuilder sbSequenceStats;
        private static StringBuilder sbTrialStats;
        private static StringBuilder sbGlobalStats;

        private static string delimiter;

        static FittsExport()
        {

            delimiter = ",";
        }

        public static void SaveState(string contrast, List<Vector2> keyframes, string resolution, string fov)
        {
            StringBuilder sbState = new StringBuilder();
            sbState.AppendLine("Contrast" + delimiter + contrast);
            sbState.AppendLine("Resolution" + delimiter + resolution);
            sbState.AppendLine("Field of view" + delimiter + fov);
            sbState.AppendLine("Number of color available" + delimiter + (keyframes == null ? 3600000 : RemainingColorCalculator.GetNumberOfColorAvailable(keyframes)));
            sbState.AppendLine("Keyframes of color hue" + delimiter + "x" + delimiter + "y");
            foreach(Vector2 key in keyframes)
            {
                sbState.AppendLine(delimiter + key.x + delimiter + key.y);
            }
            StreamWriter outStreamState = File.CreateText(path + @"\" + participantID + "_state_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");
            outStreamState.WriteLine(sbState);
            outStreamState.Close();
        }

        public static void ExportFittsToCSV()
        {
            if (exportIsOn)
            {
                CreateNewStringBuilder();
                AddStatsToStringBuilder();
                CreateFileFromStringBuilder();
            }
        }

        private static void CreateNewStringBuilder()
        {
            sbTrajectory = new StringBuilder();
            sbSequenceStats = new StringBuilder();
            sbTrialStats = new StringBuilder();
            sbGlobalStats = new StringBuilder();
        }

        private static void AddStatsToStringBuilder()
        {
            WriteTrajectoryHeading();
            WriteTrialStatsHeading();
            WriteSequenceStatsHeanding();
            WriteGlobalStatsHeading();

            foreach (SequenceStatistic stat in FittsStatistic.sequencesStats)
            {
                WriteTrajectoryCSV(stat);
                WriteTrialStatsCSV(stat);
                WriteSequenceStatsCSV(stat);
            }
            WriteGlobalStatsCSV();
        }

        private static void CreateFileFromStringBuilder()
        {
            StreamWriter outStreamTrajectory = File.CreateText(path + @"\" + participantID + "_trajectory_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");
            StreamWriter outStreamTrial = File.CreateText(path + @"\" + participantID + "_trial_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");
            StreamWriter outStreamSequence = File.CreateText(path + @"\" + participantID + "_sequence_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");
            StreamWriter outStreamGlobal = File.CreateText(path + @"\" + participantID + "_global_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");

            outStreamTrajectory.WriteLine(sbTrajectory);
            outStreamTrial.WriteLine(sbTrialStats);
            outStreamSequence.WriteLine(sbSequenceStats);
            outStreamGlobal.WriteLine(sbGlobalStats);

            outStreamTrajectory.Close();
            outStreamTrial.Close();
            outStreamSequence.Close();
            outStreamGlobal.Close();
        }

        private static void WriteTrajectoryHeading()
        {
            sbTrajectory.AppendLine("Amplitude" + delimiter
                + "Width" + delimiter
                + "Trial" + delimiter
                + "From_x" + delimiter
                + "From_y" + delimiter
                + "To_x" + delimiter
                + "To_y" + delimiter
                + "{t_x_y}");
        }

        private static void WriteTrajectoryCSV(SequenceStatistic stat)
        {
            int trial = 0;
            foreach(FittsTrial sel in stat.sequence.fittsTrials)
            {
                string row = WriteTrajectoryLine(stat, sel, trial) + delimiter + "t=";
                foreach(float time in sel.trajectory.time)
                {
                    row += delimiter + time;
                }
                sbTrajectory.AppendLine(row);

                row = WriteTrajectoryLine(stat, sel, trial) + delimiter + "x=";
                foreach (double x in sel.trajectory.x)
                {
                    row += delimiter + x;
                }
                sbTrajectory.AppendLine(row);

                row = WriteTrajectoryLine(stat, sel, trial) + delimiter + "y=";
                foreach (float y in sel.trajectory.y)
                {
                    row += delimiter + y;
                }
                sbTrajectory.AppendLine(row);

                trial++;
            }
        }

        private static string WriteTrajectoryLine(SequenceStatistic stat, FittsTrial sel, int trial)
        {
            return stat.sequence.parameter.amplitude * 2 + delimiter
                + stat.sequence.parameter.width + delimiter
                + trial + delimiter
                + sel.coord.from.x + delimiter
                + sel.coord.from.y + delimiter
                + sel.coord.to.x + delimiter
                + sel.coord.to.y;
        }

        private static void WriteTrialStatsHeading()
        {
            sbTrialStats.AppendLine("Trial" + delimiter
                + "Amplitude" + delimiter
                + "Width" + delimiter
                + "Ae" + delimiter
                + "dx" + delimiter
                + "Pointing time(s)" + delimiter
                + "Selection time(s)" + delimiter
                + "Mouvement time(s)" + delimiter
                + "Error" + delimiter
                + "Target re-entry" + delimiter
                + "Target axis crossing" + delimiter
                + "Mouvement direction change" + delimiter
                + "Orthogonal direction change" + delimiter
                + "Mouvement variability" + delimiter
                + "Mouvement error" + delimiter
                + "Mouvement offset" + delimiter);
        }

        private static void WriteTrialStatsCSV(SequenceStatistic stat)
        {
            int trialIndex = 0;
            foreach (TrialStatistic trialStat in stat.trialsStats)
            {
                sbTrialStats.AppendLine(trialIndex + delimiter
                + stat.sequence.parameter.amplitude + delimiter
                + stat.sequence.parameter.width + delimiter
                + trialStat.effectiveAmplitude + delimiter
                + trialStat.dx + delimiter
                + trialStat.trial.time.pointingTime + delimiter
                + trialStat.trial.time.selectionTime + delimiter
                + trialStat.trial.time.movementTime + delimiter
                + trialStat.trial.isError + delimiter
                + trialStat.trial.trajectory.targetReEntry + delimiter
                + trialStat.movement.taskAxisCrossing + delimiter
                + trialStat.movement.movementDirectionChange + delimiter
                + trialStat.movement.orthogonalDirectionChange + delimiter
                + trialStat.movement.movementVariable + delimiter
                + trialStat.movement.movementError + delimiter
                + trialStat.movement.movementOffset + delimiter);
                trialIndex++;
            }
        }

        private static void WriteSequenceStatsHeanding()
        {
            sbSequenceStats.AppendLine("Timestamp" + delimiter
                + "Trial" + delimiter
                + "Amplitude" + delimiter
                + "Width" + delimiter
                + "ID" + delimiter
                + "Ae" + delimiter
                + "We" + delimiter
                + "IDe(bits)" + delimiter
                + "Pointing time(s)" + delimiter
                + "Selection time(s)" + delimiter
                + "Mouvement time(s)" + delimiter
                + "ER(%)" + delimiter
                + "TP(bps)" + delimiter
                + "Target re-entry" + delimiter
                + "Target axis crossing" + delimiter
                + "Mouvement direction change" + delimiter
                + "Orthogonal direction change" + delimiter
                + "Mouvement variability" + delimiter
                + "Mouvement error" + delimiter
                + "Mouvement offset" + delimiter);
        }

        private static void WriteSequenceStatsCSV(SequenceStatistic stat)
        {
            sbSequenceStats.AppendLine(stat.sequence.timestamp + delimiter
                + stat.sequence.parameter.nbOfTarget + delimiter
                + stat.sequence.parameter.amplitude * 2 + delimiter
                + stat.sequence.parameter.width + delimiter
                + Math.Log((stat.sequence.parameter.amplitude / stat.sequence.parameter.width) + 1, 2) + delimiter
                + stat.effectiveAmplitudeMean + delimiter
                + stat.effectiveWidth + delimiter
                + stat.effectiveIndexDifficulty + delimiter
                + stat.timeMean.pointingTime + delimiter
                + stat.timeMean.selectionTime + delimiter
                + stat.timeMean.movementTime + delimiter
                + stat.errorMean + "%" + delimiter
                + stat.throughput + delimiter
                + stat.trialsStats.Sum(x => x.trial.trajectory.targetReEntry) / stat.trialsStats.Count + delimiter
                + stat.trialsStats.Sum(x => x.movement.taskAxisCrossing) / stat.trialsStats.Count + delimiter
                + stat.trialsStats.Sum(x => x.movement.movementDirectionChange) / stat.trialsStats.Count + delimiter
                + stat.trialsStats.Sum(x => x.movement.orthogonalDirectionChange) / stat.trialsStats.Count + delimiter
                + stat.trialsStats.Sum(x => x.movement.movementVariable) / stat.trialsStats.Count + delimiter
                + stat.trialsStats.Sum(x => x.movement.movementError) / stat.trialsStats.Count + delimiter
                + stat.trialsStats.Sum(x => x.movement.movementOffset) / stat.trialsStats.Count + delimiter);
        }

        private static void WriteGlobalStatsHeading()
        {
            sbGlobalStats.AppendLine("MT" + delimiter
                + "ER" + delimiter
                + "TP" + delimiter
                + "MT (regression coefficients)" + delimiter);
        }

        private static void WriteGlobalStatsCSV()
        {
            sbGlobalStats.AppendLine(FittsStatistic.movementTimeMean + delimiter
                + FittsStatistic.errorMean + delimiter
                + FittsStatistic.throughputMean + delimiter
                + "MT (regression coefficients)" + delimiter);
        }
    }
}
