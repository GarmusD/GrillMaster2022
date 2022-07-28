using GrillMaster.Data.DTO;
using GrillMaster.Data.Primitives;

namespace GrillMaster.GrillOptimizer
{
    public class GrillOptimizer
    {
        public int Width => _grillWidth;
        public int Height => _grillHeight;
        public GrillItemsList ItemsToGrill => _itemsToGrill;

        private readonly int _grillWidth;
        private readonly int _grillHeight;
        private readonly GrillItemsList _itemsToGrill;

        public GrillOptimizer(GrillOrder grillOrder)
        {
            if (grillOrder is null) throw new ArgumentNullException(nameof(grillOrder));

            _grillWidth = grillOrder.GrillSize.Width;
            _grillHeight = grillOrder.GrillSize.Height;

            _itemsToGrill = new GrillItemsList();
            _itemsToGrill.AddRange(grillOrder.MenuItems
                .SelectMany(menuItem => 
                    Enumerable.Range(0, menuItem.Count).Select(s => new GrillItem(menuItem)))
                .OrderByDescending(menuItem => menuItem.Dimensions.Squares)
                .ToList());
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
            List<OptimizedPan> optPans = grillPans
                .Select(item => CreateOptimizedPan(item))
                .ToList();
            return new() { GrillSize = new Size(_grillWidth, _grillHeight), GrilledPans = optPans };
        }

        private static OptimizedPan CreateOptimizedPan(GrillPan grillPan)
        {
            var op = new OptimizedPan();
            op.AddRange(grillPan.GrilledItems
                .Select(item => new OptimizedItem(
                                        new Rect(item.Location.X, item.Location.Y, item.Dimensions.Width, item.Dimensions.Height),
                                        item.Name))
                .ToList());
            return op;
        }
    }
}
