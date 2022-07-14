using GrillMaster2022.GrillOptimizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster2022.GrillOptimizer
{
    public class GrillItemsList : List<GrillItem>
    {
        public int UngrilledCount => this.Count(x => x.IsGrilled == false);
        public bool IsDone => !this.Any(x => x.IsGrilled == false);

        internal GrillItem? GetFittableItem(Rect areaToFit, bool rotated = false)
        {
            if (rotated)
                return this.FirstOrDefault(item => item.IsGrilled == false && areaToFit.CanFitRotated(item.Dimensions));
            else
                return this.FirstOrDefault(item => item.IsGrilled == false && areaToFit.CanFit(item.Dimensions));
        }
    }
}
