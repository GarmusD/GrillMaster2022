using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillOptimizer
{
    internal class GrillItemsList : List<GrillItem>
    {
        public int GroupsCount => _maxGroups+1;
        public int UngrilledCount => this.Count(x => x.IsGrilled == false);
        public bool IsDone => !this.Any(x => x.IsGrilled == false);

        private int _maxGroups = 0;

        public new void Add(GrillItem item)
        {
            if(item.Group > _maxGroups) _maxGroups = item.Group;
            base.Add(item);
        }

        internal GrillItem? GetUngrilledItem(int fromGroup)
        {
            return this.FirstOrDefault(x => x.Group == fromGroup && x.IsGrilled == false);
        }
    }
}
