using GrillMaster.API.DTO;
using GrillMaster.DTO;


namespace GrillMaster.GrillOptimizer
{
    public class GrillOptimizer
    {
        public int Width => _grillWidth;
        public int Height => _grillHeight;
        public GrillItemsList ItemsToGrill => _itemsToGrill;

        private readonly int _grillWidth;
        private readonly int _grillHeight;
        private readonly GrillItemsList _itemsToGrill = new();

        public GrillOptimizer(GrillOrder grillOrder)
        {
            if (grillOrder is null) throw new ArgumentNullException(nameof(grillOrder));

            _grillWidth = grillOrder.GrillSize.Width;
            _grillHeight = grillOrder.GrillSize.Height;

            grillOrder.MenuItems.Sort((a, b) => b.Dimensions.Squares.CompareTo(a.Dimensions.Squares));

            foreach (GrillOrderItem grillOrderItem in grillOrder.MenuItems)
            {
                for (var i = 0; i < grillOrderItem.Count; i++)
                    _itemsToGrill.Add( new GrillItem(grillOrderItem) );
            }
        }

        public OptimizedOrder Optimize()
        {
            List<GrillPan> panes = new();
            while (_itemsToGrill.UngrilledCount > 0)
            {
                GrillPan grillPan = new(_grillWidth, _grillHeight);
                grillPan.Grill(_itemsToGrill);
                if (grillPan.IsEmpty && _itemsToGrill.Count > 0)
                {
                    throw new GrillItemTooBigException();
                }
                panes.Add(grillPan);
            }
            return CreateOptimizedOrder(panes);
        }

        public IEnumerable<(GrillPan, int)> OptimizeStepByStep()
        {
            while (_itemsToGrill.UngrilledCount > 0)
            {
                GrillPan grillPan = new(_grillWidth, _grillHeight);
                foreach (int step in grillPan.GrillStepByStep(_itemsToGrill))
                    yield return (grillPan, step);

                if (grillPan.IsEmpty && _itemsToGrill.UngrilledCount > 0)
                {
                    throw new GrillItemTooBigException();
                }
            }
        }

        private OptimizedOrder CreateOptimizedOrder(List<GrillPan> grillPans)
        {
            List<OptimizedPan> optPans = new();
            foreach (GrillPan grillPan in grillPans)
            {
                optPans.Add(CreateOptimizedPan(grillPan));
            }
            return new(new Types.Size(_grillWidth, _grillHeight), optPans);
        }

        private OptimizedPan CreateOptimizedPan(GrillPan grillPan)
        {
            OptimizedPan optPan = new();
            foreach (GrillItem item in grillPan.GrilledItems)
            {
                OptimizedItem optItem = new(
                    new Types.Rect(item.Location.X, item.Location.Y, item.Dimensions.Width, item.Dimensions.Height),
                    item.Name
                );
                optPan.Add(optItem);
            }
            return optPan;
        }
    }
}
