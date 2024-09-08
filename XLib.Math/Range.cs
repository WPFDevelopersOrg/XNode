namespace XLib.Math
{
    public struct Range
    {
        public Range(double left, double right)
        {
            Left = left;
            Right = right;
        }

        public double Left { get; set; }

        public double Right { get; set; }
    }
}