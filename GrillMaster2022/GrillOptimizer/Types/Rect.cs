using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster2022.GrillOptimizer.Types
{
    public enum Edge
    {
        Top, Left, Right, Bottom
    }

    public struct Rect
    {
        public int Width => _width;
        public int Height => _height;
        public int Left => _left;
        public int Top => _top;
        public int Right => _left + _width;
        public int Bottom => _top + _height;
        public Point Location => new (_left, _top);
        public bool IsEmpty => _width == 0 || _height == 0;
        public int AreaSq => _width * _height;

        private readonly int _left;
        private readonly int _top;
        private readonly int _width;
        private readonly int _height;

        public static Rect Empty => new (0, 0);

        public Rect(int left, int top, int width, int height)
        {
            _left = left;
            _top = top;
            _width = width;
            _height = height;
        }

        public Rect(int width, int height) : this(0, 0, width, height) { }

        public Rect(Rect rect) : this(rect._left, rect._top, rect._width, rect._height) { }

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
            return  (rect._left < this._left + this.Width) &&
                    (this._left < rect._left + rect.Width) &&
                    (rect._top < this._top + this.Height) &&
                    (this._top < rect._top + rect.Height);
        }

        public bool Contains(Rect rect)
        {
            return  (this._left <= rect._left) &&
                    ((rect._left + rect.Width) <= (this._left + this.Width)) &&
                    (this._top <= rect._top) &&
                    ((rect._top + rect.Height) <= (this._top + this.Height));
        }

        public List<Rect> Subtract(Size dimensions)
        {
            return Subtract(new Rect(_left, _top, dimensions.Width, dimensions.Height));
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
            return edge switch
            {
                Edge.Top => GetSubtractRemainderAtTop(rect),
                Edge.Left => GetSubtractRemainderAtLeft(rect),
                Edge.Right => GetSubtractRemainderAtRight(rect),
                Edge.Bottom => GetSubtractRemainderAtBottom(rect),
                _ => Empty,
            };
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
