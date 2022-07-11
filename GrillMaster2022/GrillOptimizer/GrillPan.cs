using GrillOptimizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer
{
    internal class GrillPan
    {
        public bool IsEmpty => !grillItems.Any();

        private readonly int grillPanWidth;
        private readonly int grillPanHeight;
        private readonly List<GrillItem> grillItems = new();
        private readonly List<Rect> availableAreas = new();

        public GrillPan(int width, int height)
        {
            grillPanWidth = width;
            grillPanHeight = height;
            availableAreas.Add(new Rect(0, 0, grillPanWidth, grillPanHeight));
        }

        /// <summary>
        /// How it works: 
        /// It takes first available biggest ungrilled item and tries to fit 
        /// it into one of available areas wit enought squares.
        /// If not fitting - rotate 90 degrees and try again. 
        /// If still not fitting - take next smaller item.
        /// </summary>
        /// <param name="grillItems">List of items to grill</param>
        /// <returns></returns>
        public Task PutGrillItemsAsync(GrillItemsList grillItems)
        {
            return Task.Run(() => 
            {
                for(int i = 0; i < grillItems.GroupsCount; i++)
                {
                    GrillItem? grillItem = grillItems.GetUngrilledItem(i);
                    if(grillItem is null) continue;
                    if (TryFitGrillItem(grillItem))
                    {
                        CalculateAvailableAreas();
                    }
                    else continue;
                }
            });
        }

        private bool TryFitGrillItem(GrillItem grillItem)
        {
            foreach (var area in availableAreas)
            {
                if(area.AreaSq >= grillItem.AreaSq)
                {
                    if (area.CanFit(grillItem.Dimensions))
                    {
                        return true;
                    }
                    else if(area.CanFitRotated(grillItem.Dimensions))
                    {
                        grillItem.Rotated = true;
                    }
                }
            }
            return false;
        }

        private void CalculateAvailableAreas()
        {
            availableAreas.Clear();
        }

        private void SubtractArea()
        {

        }
    }
}
