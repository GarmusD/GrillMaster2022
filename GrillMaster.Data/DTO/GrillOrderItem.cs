using GrillMaster.Data.Primitives;

namespace GrillMaster.Data.DTO
{
    public class GrillOrderItem
    {
        public Size Dimensions { get; }
        public string Name { get; }
        public int Count { get; }

        public GrillOrderItem(Size dimensions, string name, int count)
        {
            Dimensions = dimensions;
            Name = name;
            Count = count;
        }
    }
}
