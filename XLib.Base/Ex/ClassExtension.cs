using System.Diagnostics;

namespace XLib.Base.Ex
{
    public static class ClassExtension
    {
        public static double DoubleMs(this Stopwatch stopwatch) => stopwatch.ElapsedTicks / (double)Stopwatch.Frequency * 1000;
    }
}