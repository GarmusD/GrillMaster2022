using System.Text.Json.Serialization;

namespace GrillMaster.Client.DTO
{
    public struct SimpleRect
    {
        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }

        [JsonConstructor]
        public SimpleRect(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }
}
