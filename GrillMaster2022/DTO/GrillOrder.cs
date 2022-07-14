using GrillMaster2022.GrillOptimizer.Types;


namespace GrillMaster2022.DTO
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
