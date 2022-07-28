using GrillMaster.CLI.AppConfiguration;
using GrillMaster.Data.DTO;

namespace GrillMaster.CLI.Renderer
{
    internal class Renderer
    {
        public static void RenderOutput(OutputOptions outputOptions, OptimizedOrder optimizedOrder)
        {
            GetRenderer(outputOptions.Output).Render(outputOptions, optimizedOrder);
        }

        private static IRenderer GetRenderer(OutputTarget output)
        {
            return output switch
            {
                OutputTarget.Console => new Console.ConsoleRenderer(),
                OutputTarget.Json => new Json.JsonRenderer(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
