namespace GrillMaster.Client.DTO
{
    public class OptimizedItem
    {
        public string Name { get; }
        public SimpleRect Location { get; }

        public OptimizedItem(SimpleRect location, string name)
        {
            Location = location;
            Name = name;
        }
    }
}
