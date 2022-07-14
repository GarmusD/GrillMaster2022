using System;
using System.Text.Json.Serialization;

namespace GrillMaster2022.GrillOptimizer.Types
{
    public struct Size
    {
        public int Width { get; }
        public int Height { get; }
        public bool IsEmpty => Width == 0 || Height == 0;
        public int Squares => Width * Height;

        [JsonConstructor]
        public Size(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));
            Width = width;
            Height = height;
        }
    }
}
