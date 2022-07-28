using GrillMaster.Data.Primitives;

namespace GrillMaster.CLI.Renderer
{
    internal static class RectExt
    {
        public static bool Contains(this Rect rect, int x, int y)
        {
            return ((x >= rect.Left) && (x - rect.Width <= rect.Left) &&
                    (y >= rect.Top) && (y - rect.Height <= rect.Top));
        }
    }
}
