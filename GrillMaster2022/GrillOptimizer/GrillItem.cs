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
        public int Group => _group;
        public int ID => _id;
        public int Width => _width;
        public int Height => _height;
        public string Name => _name;
        public bool Rotated { get => _rotated; set => SetRotation(value); }
        public bool IsGrilled => _x > -1 && _y > -1;
        public int AreaSq => _width * _height;
        public Size Dimensions => new(_width, _height);
        public Point Location => new(_x, _y);

        private readonly int _group;
        private readonly int _id;
        private int _x, _y, _width, _height;
        private readonly string _name;
        private bool _rotated = false;

        public GrillItem(int group, int id, GrillOrderItem grillOrderItem)
        {
            _group = group;
            _id = id;
            _x = _y = -1;
            _width = grillOrderItem.Dimensions.Width;
            _height = grillOrderItem.Dimensions.Height;
            _name = grillOrderItem.Name ?? string.Empty;
        }

        private void SetRotation(bool value)
        {
            if(value != _rotated)
            {
                _rotated = value;
                (_width, _height) = (_height, _width);
            }
        }

        public void SetItemGrilled(Point location)
        {
            _x = location.X;
            _y = location.Y;
        }
    }
}
