using GrillOptimizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer
{
    internal class GrillItem
    {
        public int Group => group;
        public int ID => id;
        public int Width => width;
        public int Height => height;
        public string Name => name;
        public bool Rotated { get => rotation; set => SetRotation(value); }
        public bool IsGrilled => x > -1 && y > -1;
        public int AreaSq => width * height;
        public Size Dimensions => new(width, height);
        public Point Location => new(x, y);

        private readonly int group;
        private readonly int id;
        private int x, y, width, height;
        private readonly string name;
        private bool rotation = false;

        public GrillItem(int group, int id, GrillOrderItem grillOrderItem)
        {
            this.group = group;
            this.id = id;
            x = y = -1;
            width = grillOrderItem.Dimensions.Width;
            height = grillOrderItem.Dimensions.Height;
            name = grillOrderItem.Name ?? string.Empty;
        }

        private void SetRotation(bool value)
        {
            if(value != rotation)
            {
                rotation = value;
                (width, height) = (height, width);
            }
        }

        public void SetItemGrilled(Point location)
        {
            x = location.X;
            y = location.Y;
        }
    }
}
