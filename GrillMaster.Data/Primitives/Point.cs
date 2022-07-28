namespace GrillMaster.Data.Primitives
{

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point()
        {
            X = Y = 0;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
