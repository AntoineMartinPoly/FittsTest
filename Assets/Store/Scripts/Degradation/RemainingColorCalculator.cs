using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Fitts
{
    static class RemainingColorCalculator
    {
        public static int GetNumberOfColorAvailable(List<Vector2> keyframes)
        {
            if (keyframes.Select(key => FloatComparator.IsEqual(key.y, 0.5f)).Count() != 100)
                // The keyframes are represented with Vector2 because only the x and y values are useful here
                return GetRGBColorListFromHueOfKeyframe(GetEqualConsecutiveAndNeutralKeyframe(keyframes));
            return 3600000;
        }


        private static List<Vector2> GetEqualConsecutiveAndNeutralKeyframe(List<Vector2> keyframes)
        {
            // The List returned is a range of consecutive keyframes with the same y value or a neutral y value of 0.5f (no shifting)
            // Consecutive y values are wanted because they are the only HUE color left that are not shifted to the initial color
            List<Vector2> consecutiveKeyframes = new List<Vector2>();

            float lastKeyYValue = keyframes[0].y;
            // y value of 0.5 are kept because they are the initial colors we shift the other colors to
            foreach (Vector2 keyframe in keyframes)
            {
                if (FloatComparator.IsEqual(keyframe.y, lastKeyYValue) || FloatComparator.IsEqual(keyframe.y, 0.5f))
                    consecutiveKeyframes.Add(keyframe);
                lastKeyYValue = keyframe.y;
            }
            return consecutiveKeyframes;
        }

        private static int GetRGBColorListFromHueOfKeyframe(List<Vector2> keyframes)
        {
            int remainingColors = 0;

            // In case no color was removed
            if (keyframes.Count == 100) return 3600000;

            // HSV Color have a value between 1 and 360 and to get all remaining colors
            // every possible hue value between the intervals of keyframes are needed
            foreach (List<Vector2> keyframesInterval in GetKeyframesIntervalOfRemainingColor(keyframes))
            {
                float hueValue = keyframesInterval[0].x;

                while (hueValue <= keyframesInterval.Last().x)
                {
                    // For each saturation (S)
                    for (float i = 1; i <= 100; i++)
                    {
                        // For each lightness/value (V)
                        for (float y = 1; y <= 100; y++)
                        {
                            remainingColors++;
                        }
                    }
                    hueValue += 1f / 360f;
                }
            }
            return remainingColors;
        }

        private static List<List<Vector2>> GetKeyframesIntervalOfRemainingColor(List<Vector2> keyframes)
        {
            Vector2 lastKeyframe = keyframes[0];
            // Group keyframe with the same y value together to create interval of keyframes
            List<List<Vector2>> keyframesIntervalOfRemainingColor = new List<List<Vector2>>();
            List<Vector2> currentInterval = new List<Vector2>();
            foreach (Vector2 keyframe in keyframes)
            {
                if (keyframe.y == 0.5f)
                {
                    keyframesIntervalOfRemainingColor.Add(new List<Vector2> { keyframe });
                    if(currentInterval.Count != 0)
                    {
                        keyframesIntervalOfRemainingColor.Add(currentInterval);
                        currentInterval.Clear();
                    }
                }
                else if (FloatComparator.IsEqual(keyframe.y, lastKeyframe.y))
                {
                    if (!currentInterval.Contains(lastKeyframe)) currentInterval.Add(lastKeyframe);
                    currentInterval.Add(lastKeyframe);
                } 
                else
                {
                    if (currentInterval.Count != 0)
                    {
                        keyframesIntervalOfRemainingColor.Add(currentInterval);
                        currentInterval.Clear();
                    }
                }
                lastKeyframe = keyframe;
            }
            return keyframesIntervalOfRemainingColor;
        }
    }
}
