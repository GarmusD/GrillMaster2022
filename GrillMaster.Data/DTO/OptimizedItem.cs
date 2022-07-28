using GrillMaster.Data.Primitives;

namespace GrillMaster.Data.DTO
{
    public class OptimizedItem
    {
        public string Name { get; }
        public Rect Location { get; }

        public OptimizedItem(Rect location, string name)
        {
            Location = location;
            Name = name;
        }
    }
}
