using GrillMaster.GrillOptimizer.Types;

namespace GrillMaster.API.DTO
{
    public class OptimizedOrder
    {
        public Size GrillSize { get; }
        public List<OptimizedPan> GrilledPans { get; }

        public OptimizedOrder(Size grillSize, List<OptimizedPan> grillPans)
        {
            GrillSize = grillSize;
            GrilledPans = grillPans;
        }
    }
}
