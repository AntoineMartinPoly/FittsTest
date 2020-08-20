using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Fitts
{
    // Class in charge of exporting Eye data to csv
    static class EyeExport
    {
        public static bool exportIsOn;

        private static StringBuilder sbGaze;
        private static StringBuilder sbEye;

        private static string delimiter;
        static EyeExport()
        {
            delimiter = ",";
        }

        public static void ExportEyeToCSV()
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
            sbGaze = new StringBuilder();
            sbEye = new StringBuilder();
        }

        private static void AddStatsToStringBuilder()
        {
            WriteGazeHeading();
            WriteEyeHeading();

            WriteGazeCSV();
            WriteEyeCSV();
        }

        private static void CreateFileFromStringBuilder()
        {
            StreamWriter outStreamGaze = File.CreateText(FittsExport.path + @"\" + FittsExport.participantID + "_gaze_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");
            StreamWriter outStreamEye = File.CreateText(FittsExport.path + @"\" + FittsExport.participantID + "_eye_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".csv");
            outStreamGaze.WriteLine(sbGaze);
            outStreamEye.WriteLine(sbEye);

            outStreamGaze.Close();
            outStreamEye.Close();
        }

        private static void WriteGazeHeading()
        {
            sbGaze.AppendLine("Amplitude" + delimiter
                + "Width" + delimiter
                + "Selection Order" + delimiter
                + "Gaze Time (s)" + delimiter
                + "Number of focus");
        }

        private static void WriteGazeCSV()
        {
            foreach (SequenceStatistic sequenceStats in FittsStatistic.sequencesStats)
            {
                for (int i = 0; i < sequenceStats.eyeData.Length; i++)
                {
                    sbGaze.AppendLine(sequenceStats.sequence.parameter.amplitude + delimiter
                        + sequenceStats.sequence.parameter.width + delimiter
                        + GetTargetSelectionIndex(i, sequenceStats.sequence.parameter.nbOfTarget) + delimiter
                        + sequenceStats.eyeData[i].gazeTime + delimiter
                        + sequenceStats.eyeData[i].nbOfFocus);
                }
            }
        }

        private static void WriteEyeHeading()
        {
            sbEye.AppendLine("Timestamp" + delimiter
                + "Left Pupil diameter (mm)" + delimiter
                + "Right Pupil diameter (mm)" + delimiter
                + "Combined Pupil diameter (mm)" + delimiter
                + "Left Pupil origin" + delimiter
                + "Right Pupil origin" + delimiter
                + "Combined Pupil origin" + delimiter
                + "Combined Convergence Distance (mm)" + delimiter
                + "Combined Convergence Validity" + delimiter);
        }

        private static void WriteEyeCSV()
        {
            EyeTracking eyeTracking = GameObject.Find("Fitts").GetComponent<EyeTracking>();
            foreach (EyeTracking.EyeExportStatistic data in eyeTracking.eyeStatistic)
            {
                sbEye.AppendLine(data.timestamp + delimiter
                    + data.leftEye.pupil_diameter_mm + delimiter
                    + data.rightEye.pupil_diameter_mm + delimiter
                    + data.combinedEye.eye_data.pupil_diameter_mm + delimiter
                    + data.leftEye.gaze_origin_mm.x + ":" + data.leftEye.gaze_origin_mm.y + ":" + data.leftEye.gaze_origin_mm.z + ":" + delimiter
                    + data.rightEye.gaze_origin_mm.x + ":" + data.rightEye.gaze_origin_mm.y + ":" + data.rightEye.gaze_origin_mm.z + ":" + delimiter
                    + data.combinedEye.eye_data.gaze_origin_mm.x + ":" + data.combinedEye.eye_data.gaze_origin_mm.y + ":" + data.combinedEye.eye_data.gaze_origin_mm.z + ":" + delimiter
                    + data.combinedEye.convergence_distance_mm + delimiter
                    + data.combinedEye.convergence_distance_validity);
            }
        }

        private static int GetTargetSelectionIndex(int i, int nbOfTarget)
        {
            return (i + (nbOfTarget / 2) + 1) % nbOfTarget;
        }
    }
}
