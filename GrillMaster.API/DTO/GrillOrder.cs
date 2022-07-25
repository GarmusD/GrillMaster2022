using GrillMaster.GrillOptimizer.Types;


namespace GrillMaster.DTO
{
    public class GrillOrder
    {
        public Size GrillSize { get; }
        public List<GrillOrderItem> MenuItems { get; }

        public GrillOrder(Size grillSize, List<GrillOrderItem> menuItems)
        {
            GrillSize = grillSize;
            MenuItems = menuItems;
        }
    }
}
