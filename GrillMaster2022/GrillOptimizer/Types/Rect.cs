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
        public int X => _x;
        public int Y => _y;
        public int Width => _width;
        public int Height => _height;
        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;
        public Point Location => new Point(X, Y);

        public bool IsEmpty => _width == 0 || _height == 0;
        public int AreaSq => _width * _height;

        private readonly int _x;
        private readonly int _y;
        private readonly int _width;
        private readonly int _height;

        public static Rect Empty => new (0, 0);

        public Rect(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public Rect(int width, int height) : this(0, 0, width, height) { }

        public Rect(Rect rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public bool CanFit(Size area)
        {
            (int w, int h) = ((int)area.Width, (int)area.Height);
            return _width >= w && _height >= h;
        }

        public bool CanFitRotated(Size area)
        {
            (int w, int h) = ((int)area.Height, (int)area.Width);
            return _width >= w && _height >= h;
        }

        public bool IntersectsWith(Rect rect)
        {
            return  (rect.X < this.X + this.Width) &&
                    (this.X < rect.X + rect.Width) &&
                    (rect.Y < this.Y + this.Height) &&
                    (this.Y < rect.Y + rect.Height);
        }

        public bool Contains(Rect rect)
        {
            return  (this.X <= rect.X) &&
                    ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                    (this.Y <= rect.Y) &&
                    ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        public List<Rect> Subtract(Size dimensions)
        {
            return Subtract(new Rect(_x, _y, dimensions.Width, dimensions.Height));
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
                    return Empty;
            }
        }

        private Rect GetSubtractRemainderAtTop(Rect rect)
        {
            if (rect.Top <= Top) return Empty;
            return new Rect(Left, Top, Width, rect.Top - Top);
        }
        
        private Rect GetSubtractRemainderAtLeft(Rect rect)
        {
            if (rect.Left <= Left) return Empty;
            return new Rect(Left, Top, rect.Left - Left, Height);
        }

        private Rect GetSubtractRemainderAtRight(Rect rect)
        {
            if (Right <= rect.Right) return Empty;
            return new Rect(rect.Right, Top, Right - rect.Right, Height);
        }

        private Rect GetSubtractRemainderAtBottom(Rect rect)
        {
            if (Bottom <= rect.Bottom) return Empty;
            return new Rect(Left, rect.Bottom, Width, Bottom - rect.Bottom);
        }
    }
}
