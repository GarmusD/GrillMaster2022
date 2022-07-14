using GrillMaster2022.GrillOptimizer.Types;

namespace GrillMaster2022.GrillOptimizer
{
    public class GrillPan
    {
        public int Width => _grillPanWidth;
        public int Height => _grillPanHeight;
        public bool IsEmpty => !_grilledItems.Any();
        public List<GrillItem> GrilledItems => _grilledItems;
        public GrillItemsList? ItemsToGrill => _itemsToGrill;
#if DEBUG
        public List<Rect> AvailableAreas => _availableAreas;
#endif
        private readonly int _grillPanWidth;
        private readonly int _grillPanHeight;
        private readonly List<GrillItem> _grilledItems = new();
        private readonly List<Rect> _availableAreas = new();
        private GrillItemsList? _itemsToGrill = null;

        public GrillPan(int width, int height)
        {
            _grillPanWidth = width;
            _grillPanHeight = height;
            _availableAreas.Add(new Rect(0, 0, _grillPanWidth, _grillPanHeight));
        }

        /// <summary>
        /// How it works: 
        /// It takes first topmost available area and tries to fit 
        /// biggest ungrilled item.
        /// If none fits - try fit items rotated by 90 degrees. 
        /// If none fits anyway - here is done.
        /// </summary>
        /// <param name="grillItems">List of items to grill</param>
        /// <returns></returns>
        public void Grill(GrillItemsList grillItems)
        {
            _itemsToGrill = grillItems;
            while (TryFitGrillItem()) {}
        }

        public IEnumerable<int> GrillStepByStep(GrillItemsList grillItems)
        {
            _itemsToGrill = grillItems;
            int currentStep = 0;
            while (TryFitGrillItem())
            {
                yield return currentStep++;
            }
        }

        private bool TryFitGrillItem()
        {
            foreach (Rect availArea in _availableAreas)
            {
                GrillItem? itemToGrill;
                if ((itemToGrill = _itemsToGrill?.GetFittableItem(availArea)) is not null)
                {
                    FitGrillItem(availArea, itemToGrill);
                    return true;
                }
                else if ((itemToGrill = _itemsToGrill?.GetFittableItem(availArea, true)) is not null)
                {
                    itemToGrill.Rotated = true;
                    FitGrillItem(availArea, itemToGrill);
                    return true;
                }
            }
            return false;
        }

        private void FitGrillItem(Rect area, GrillItem grillItem)
        {
            grillItem.SetItemGrilled(area.Location);
            _grilledItems.Add(grillItem);
            FindAndSubractIntersectedAreas(grillItem.UsedArea);
            PostProcessAvailableAreas();
        }

        private void PostProcessAvailableAreas()
        {
            EatInnerAreas();
            SortAvailableAreasByYX();
        }
        
        private void EatInnerAreas()
        {
            SortAvailableAreasBySq();
            List<Rect> innerAreas = new();
            foreach (var area in _availableAreas)
            {
                innerAreas.AddRange(_availableAreas.Where(x => !x.Equals(area) && area.Contains(x)).ToList());
            }
            foreach (var area in innerAreas)
            {
                _availableAreas.Remove(area);
            }
        }

        private void FindAndSubractIntersectedAreas(Rect grilledItemArea)
        {
            var intersectedAreas = _availableAreas.Where(area => area.IntersectsWith(grilledItemArea)).ToList();
            List<Rect> subtractedAreas = new();
            foreach (var area in intersectedAreas)
            {
                _availableAreas.Remove(area);
                subtractedAreas.AddRange(area.Subtract(grilledItemArea));
            }
            _availableAreas.AddRange(subtractedAreas);
        }

        private void SortAvailableAreasBySq()
        {
            _availableAreas.Sort((a, b) => { return b.AreaSq.CompareTo(a.AreaSq); });
        }

        private void SortAvailableAreasByYX()
        {
            _availableAreas.Sort((a, b) => 
            { 
                var result = a.Location.Y.CompareTo(b.Location.Y);
                if (result == 0) result = a.Location.X.CompareTo(b.Location.X);
                return result;
            });
        }

    }
}
