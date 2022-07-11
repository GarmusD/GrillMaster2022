using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer

{
    internal class GrillOptimizer
    {
        public int Width => grillWidth;
        public int Height => grillHeight;
        public List<GrillItem> ItemsToGrill => itemsToGrill;

        private readonly int grillWidth;
        private readonly int grillHeight;
        private readonly List<GrillItem> itemsToGrill = new ();

        public GrillOptimizer(GrillOrder grillOrder)
        {
            if (grillOrder is null) throw new ArgumentNullException(nameof(grillOrder));

            grillWidth = grillOrder.GrillSize.Width;
            grillHeight = grillOrder.GrillSize.Height;

            int grillItemGroup = 0;
            foreach (var grillOrderItem in grillOrder.MenuItems)
            {
                for (int i = 0; i < grillOrderItem.Count; i++)
                {
                    GrillItem grillItem = new(grillItemGroup, i, grillOrderItem);
                    itemsToGrill.Add(grillItem);
                }
                grillItemGroup++;
            }

            itemsToGrill.Sort((a, b) => { return a.AreaSq.CompareTo(b.AreaSq); });
        }

    }
}
