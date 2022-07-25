namespace GrillMaster.Client.DTO
{
    public class OptimizedOrder
    {
        public SimpleSize GrillSize { get; }
        public List<OptimizedPan>? GrilledPans { get; }
        public OptimizedOrder(SimpleSize grillSize, List<OptimizedPan>? grilledPans)
        {
            GrillSize = grillSize;
            GrilledPans = grilledPans;
        }
    }
}
