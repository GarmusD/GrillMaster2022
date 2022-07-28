using GrillMaster.Data.Primitives;
using System.Text.Json.Serialization;

namespace GrillMaster.Data.DTO
{
    public class OptimizedOrder
    {
        public Size GrillSize { get; init; }
        public List<OptimizedPan>? GrilledPans { get; init; }
    }
}