namespace Assets.Scripts.Fitts
{
    // Compare float considering the 6-9 digits precision of float type
    static class FloatComparator
    {
        public static bool IsEqual(float a, float b)
        {
            return a <= b + 0.0000005f && a >= b - 0.0000005f;
        }

        public static bool FirstIsLessThanSecond(float a, float b)
        {
            return a < b - 0.0000005f;
        }

        public static bool FirstIsMoreThanSecond(float a, float b)
        {
            return a > b + 0.0000005f;
        }
    }
}
