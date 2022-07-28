using GrillMaster.CLI.AppConfiguration;
using GrillMaster.Data.DTO;

namespace GrillMaster.CLI.Renderer
{
    internal interface IRenderer
    {
        void Render(OutputOptions outputOptions, OptimizedOrder optimizedOrder);
    }
}
