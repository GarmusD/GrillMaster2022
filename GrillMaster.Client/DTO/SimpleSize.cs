using System.Text.Json.Serialization;

namespace GrillMaster.Client.DTO
{
    public struct SimpleSize
    {
        public int Width { get; }
        public int Height { get; }

        [JsonConstructor]
        public SimpleSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
