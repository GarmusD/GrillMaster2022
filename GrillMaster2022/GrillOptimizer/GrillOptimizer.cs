using GrillMaster2022.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster2022.GrillOptimizer

{
    public class GrillOptimizer
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
        }

        public List<GrillPan> Run()
        {
            List<GrillPan> panes = new ();
            while (_itemsToGrill.UngrilledCount > 0)
            {
                GrillPan grillPan = new (_grillWidth, _grillHeight);
                grillPan.Grill(_itemsToGrill);
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
