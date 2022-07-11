using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GrillOptimizer.Types;


namespace GrillOptimizer
{
    internal class GrillOrder
    {
        public Size GrillSize { get; }
        public List<GrillOrderItem> MenuItems { get; }

        [JsonConstructor]
        public GrillOrder(Size grillSize, List<GrillOrderItem> menuItems)
        {
            GrillSize = grillSize;
            MenuItems = menuItems;
        }
    }

    internal class GrillOrderItem
    {
        public Size Dimensions { get; }
        public string Name { get; }
        public int Count { get; }

        [JsonConstructor]
        public GrillOrderItem(Size dimensions, string name, int count)
        {
            Dimensions = dimensions;
            Name = name;
            Count = count;
        }
    }
}
