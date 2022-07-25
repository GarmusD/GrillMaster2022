using GrillMaster.Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Renderer
{
    internal static class SimpleRectExt
    {
        public static bool Contains(this SimpleRect rect, int x, int y)
        {
            return ((x >= rect.Left) && (x - rect.Width <= rect.Left) &&
                    (y >= rect.Top) && (y - rect.Height <= rect.Top));
        }
    }
}
