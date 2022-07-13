using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer

{
    internal class GrillOptimizer
    {
        public int Width => _grillWidth;
        public int Height => _grillHeight;
        public GrillItemsList ItemsToGrill => _itemsToGrill;

        private readonly int _grillWidth;
        private readonly int _grillHeight;
        private readonly GrillItemsList _itemsToGrill = new ();

        public GrillOptimizer(GrillOrder grillOrder)
        {
            if (grillOrder is null) throw new ArgumentNullException(nameof(grillOrder));

            _grillWidth = grillOrder.GrillSize.Width;
            _grillHeight = grillOrder.GrillSize.Height;

            grillOrder.MenuItems.Sort((a, b) => b.Dimensions.Squares.CompareTo(a.Dimensions.Squares));

            int grillItemGroup = 0;
            foreach (GrillOrderItem grillOrderItem in grillOrder.MenuItems)
            {
                for (var i = 0; i < grillOrderItem.Count; i++)
                {
                    GrillItem grillItem = new(grillItemGroup, i, grillOrderItem);
                    _itemsToGrill.Add(grillItem);
                }
                grillItemGroup++;
            }

            //_itemsToGrill.Sort((a, b) => { return b.AreaSq.CompareTo(a.AreaSq); });
        }

        public async Task<List<GrillPan>> RunAsync()
        {
            List<GrillPan> panes = new List<GrillPan>();
            while (_itemsToGrill.UngrilledCount > 0)
            {
                GrillPan grillPan = new (_grillWidth, _grillHeight);
                await grillPan.GrillAsync(_itemsToGrill);
                if (grillPan.IsEmpty && _itemsToGrill.Count > 0)
                {
                    throw new GrillItemTooBigException();
                }
                panes.Add(grillPan);
            }
            return panes;
        }

        public IEnumerable<(GrillPan, int)> RunStepByStep()
        {
            while (_itemsToGrill.UngrilledCount > 0)
            {
                GrillPan grillPan = new(_grillWidth, _grillHeight);
                foreach(int step in grillPan.GrillStepByStep(_itemsToGrill))
                    yield return (grillPan, step);

                if (grillPan.IsEmpty && _itemsToGrill.UngrilledCount > 0)
                {
                    throw new GrillItemTooBigException();
                }
            }
        }
    }
}
