using GrillMaster2022.DTO;
using GrillMaster2022.GrillOptimizer.Types;

namespace GrillMaster2022.GrillOptimizer
{
    public class GrillItem
    {
        public int Group => _group;
        public int ID => _id;
        public string Name => _name;
        public bool Rotated { get => _rotated; set => SetRotated(value); }
        public bool IsGrilled => _location.X > -1 && _location.Y > -1;
        public int AreaSq => _dimensions.Width * _dimensions.Height;
        public Size Dimensions => _dimensions;
        public Point Location => _location;
        public Rect UsedArea => new (_location.X, _location.Y, _dimensions.Width, _dimensions.Height);


        private readonly int _group;
        private readonly int _id;
        private Point _location;
        private Size _dimensions;
        private readonly string _name;
        private bool _rotated = false;

        public GrillItem(int group, int id, GrillOrderItem grillOrderItem)
        {
            _group = group;
            _id = id;
            _location = new (-1, -1);
            _dimensions = new (grillOrderItem.Dimensions.Width, grillOrderItem.Dimensions.Height);
            _name = grillOrderItem.Name ?? string.Empty;
        }

        private void SetRotated(bool value)
        {
            if(value != _rotated)
            {
                _rotated = value;
                //swap width and height
                _dimensions = new(_dimensions.Height, _dimensions.Width);
            }
        }

        public void SetItemGrilled(Point location)
        {
            _location = location;
        }
    }
}
