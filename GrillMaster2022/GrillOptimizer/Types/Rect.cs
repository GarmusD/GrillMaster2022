using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer.Types
{
    internal enum Edge
    {
        Top, Left, Right, Bottom
    }

    internal struct Rect
    {
        public int X => x;
        public int Y => y;
        public int Width => width;
        public int Height => height;
        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;
        public Point Location => new Point(X, Y);

        public bool IsEmpty => width == 0 || height == 0;
        public int AreaSq => width * height;

        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        public static Rect Empty() => new (0, 0);

        public Rect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rect(int width, int height) : this(0, 0, width, height) { }

        public Rect(Rect rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public bool CanFit(Size area)
        {
            (int w, int h) = ((int)area.Width, (int)area.Height);
            return width >= w && height >= h;
        }

        public bool CanFitRotated(Size area)
        {
            (int w, int h) = ((int)area.Height, (int)area.Width);
            return width >= w && height >= h;
        }

        public bool IntersectsWith(Rect rect)
        {
            return (rect.X < this.X + this.Width) &&
            (this.X < (rect.X + rect.Width)) &&
            (rect.Y < this.Y + this.Height) &&
            (this.Y < rect.Y + rect.Height);
        }

        public List<Rect> Subtract(Rect area)
        {
            List<Rect> results = new ();
            foreach (Edge edge in Enum.GetValues(typeof(Edge)))
            {
                var result = GetSubtractReminderAtEdge(area, edge);
                if (!result.IsEmpty) results.Add(result);
            }
            return results;
        }

        private Rect GetSubtractReminderAtEdge(Rect rect, Edge edge)
        {
            switch (edge)
            {
                case Edge.Top:
                    return GetSubtractRemainderAtTop(rect);
                case Edge.Left:
                    return GetSubtractRemainderAtLeft(rect);
                case Edge.Right:
                    return GetSubtractRemainderAtRight(rect);
                case Edge.Bottom:
                    return GetSubtractRemainderAtBottom(rect);
                default:
                    return Empty();
            }
        }

        private Rect GetSubtractRemainderAtTop(Rect rect)
        {
            if (rect.Top <= Top) return Empty();
            return new Rect(Left, Top, Width, rect.Top);
        }
        
        private Rect GetSubtractRemainderAtLeft(Rect rect)
        {
            if (rect.Left <= Left) return Empty();
            return new Rect(Left, Top, rect.Left - Left, Height);
        }

        private Rect GetSubtractRemainderAtRight(Rect rect)
        {
            if (Right <= rect.Right) return Empty();
            return new Rect(rect.Right, Top, Right - rect.Right, Height);
        }

        private Rect GetSubtractRemainderAtBottom(Rect rect)
        {
            if (Bottom <= rect.Bottom) return Empty();
            return new Rect(Left, rect.Bottom, Width, Bottom - rect.Bottom);
        }
    }
}
