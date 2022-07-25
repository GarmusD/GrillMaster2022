using System.Text.Json.Serialization;


namespace GrillMaster.GrillOptimizer.Types
{
    public struct Size
    {
        public int Width { get; }
        public int Height { get; }
        [JsonIgnore]
        public bool IsEmpty => Width == 0 || Height == 0;
        [JsonIgnore]
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
