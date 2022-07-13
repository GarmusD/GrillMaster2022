using GrillOptimizer.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer
{
    internal class GrillPan
    {
        public int Width => _grillPanWidth;
        public int Height => _grillPanHeight;
        public bool IsEmpty => !_grilledItems.Any();
        public List<GrillItem> GrilledItems => _grilledItems;
        public GrillItemsList? ItemsToGrill { get; private set; } = null;
#if DEBUG
        public List<Rect> AvailableAreas => _availableAreas;
#endif
        private readonly int _grillPanWidth;
        private readonly int _grillPanHeight;
        private readonly List<GrillItem> _grilledItems = new();
        private readonly List<Rect> _availableAreas = new();

        public GrillPan(int width, int height)
        {
            _grillPanWidth = width;
            _grillPanHeight = height;
            _availableAreas.Add(new Rect(0, 0, _grillPanWidth, _grillPanHeight));
        }

        /// <summary>
        /// How it works: 
        /// It takes first available biggest ungrilled item and tries to fit 
        /// it into one of available areas with enought squares.
        /// If not fitting - rotate 90 degrees and try again. 
        /// If still not fitting - take next smaller item.
        /// </summary>
        /// <param name="grillItems">List of items to grill</param>
        /// <returns></returns>
        public Task GrillAsync(GrillItemsList grillItems)
        {
            ItemsToGrill = grillItems;
            return Task.Run(() => 
            {
                for(int group = 0; group < grillItems.GroupsCount; group++)
                {
                    GrillItem? grillItem = grillItems.GetUngrilledItem(group);
                    while (grillItem is not null)
                    {
                        if (!TryFitGrillItem(grillItem))
                        {
                            break;
                        }
                        grillItem = grillItems.GetUngrilledItem(group);
                    }
                }
            });
        }

        public IEnumerable<int> GrillStepByStep(GrillItemsList grillItems)
        {
            ItemsToGrill = grillItems;
            int currentStep = 0;
            for (int group = 0; group < grillItems.GroupsCount; group++)
            {
                GrillItem? grillItem = grillItems.GetUngrilledItem(group);
                while (grillItem is not null)
                {
                    if (!TryFitGrillItem(grillItem))
                    {
                        break;
                    }
                    yield return currentStep++;
                    grillItem = grillItems.GetUngrilledItem(group);
                }
            }
        }
        

        private bool TryFitGrillItem(GrillItem grillItem)
        {
            foreach (Rect area in _availableAreas)
            {
                if(area.AreaSq >= grillItem.AreaSq)
                {
                    bool doFit = false;
                    if (area.CanFit(grillItem.Dimensions))
                    {
                        doFit = true;
                    }
                    else if(area.CanFitRotated(grillItem.Dimensions))
                    {
                        grillItem.Rotated = true;
                        doFit = true;
                    }

                    if(doFit)
                    {
                        FitGrillItem(area, grillItem);
                        return true;
                    }
                }
            }
            return false;
        }

        private void FitGrillItem(Rect area, GrillItem grillItem)
        {
            grillItem.SetItemGrilled(area.Location);
            _grilledItems.Add(grillItem);
            List<Rect> subtractedAreas = area.Subtract(grillItem.Dimensions);
            // area is used, remove it from list
            _availableAreas.Remove(area);
            // find other intersected areas
            subtractedAreas.AddRange(FindAndSubractIntersectedAreas(grillItem.UsedArea));
            _availableAreas.AddRange(subtractedAreas);
            // merge inner areas, expand overlapped
            PostProcessAvailableAreas();
        }

        private void PostProcessAvailableAreas()
        {
            SortAvailableAreasBySq();
            EatInnerAreas();
            SortAvailableAreasByYX();
        }

        private List<Rect> FindAndSubractIntersectedAreas(Rect grilledItemArea)
        {
            List<Rect> result = new ();
            Rect intersectedArea = FindIntersectedArea(grilledItemArea);
            while(!intersectedArea.IsEmpty)
            {
                _availableAreas.Remove(intersectedArea);
                result.AddRange(intersectedArea.Subtract(grilledItemArea));
                intersectedArea = FindIntersectedArea(grilledItemArea);
            }
            return result;
        }

        private Rect FindIntersectedArea(Rect grilledItemArea)
        {
            foreach (Rect area in _availableAreas)
            {
                if(area.IntersectsWith(grilledItemArea))
                    return area;
            }
            return Rect.Empty;
        }

        private void EatInnerAreas()
        {
            System.Diagnostics.Debug.WriteLine("EatInnerAreas()");
            bool done = false;
            int diagItersTotal = 0;
            while (!done)
            {
                System.Diagnostics.Debug.WriteLine("Starting check of areas...");
                int diagIters = 0;                
                done = true;
                for(int i = 0; i < _availableAreas.Count - 1; i++)
                {
                    Rect area = _availableAreas[i];
                    for (int j = i + 1; j < _availableAreas.Count; j++)
                    {
                        diagIters++;
                        diagItersTotal++;
                        Rect innerArea = _availableAreas[j];
                        //System.Diagnostics.Debug.WriteLine($"Comapring [(X:{area.X},Y:{area.Y}), (W:{area.Width},H:{area.Height})] vs [(X:{innerArea.X},Y:{innerArea.Y}), (W:{innerArea.Width},H:{innerArea.Height})]");
                        if(area.Contains(innerArea))
                        {
                            System.Diagnostics.Debug.WriteLine($"'area' contains 'innerArea'. Removing 'innerArea' and re-checking. (Done in {diagIters} iterations)");
                            done = false;
                            _availableAreas.Remove(innerArea);
                            break;
                        }
                    }
                    if (!done) break;
                }
            }
            System.Diagnostics.Debug.WriteLine($"EatInnerAreas() done. Total iterations: {diagItersTotal}");
        }

        private void SortAvailableAreasBySq()
        {
            _availableAreas.Sort((a, b) => { return b.AreaSq.CompareTo(a.AreaSq); });
        }

        private void SortAvailableAreasByYX()
        {
            //_availableAreas.Sort((a, b) => { return a.AreaSq.CompareTo(b.AreaSq); });
            _availableAreas.Sort((a, b) => { return a.X.CompareTo(b.X); });
            _availableAreas.Sort((a, b) => { return a.Y.CompareTo(b.Y); });
            
        }

    }
}
