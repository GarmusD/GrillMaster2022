using GrillMaster.Client.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Renderer
{
    internal class Renderer
    {
        public static void RenderOutput(OutputOptions outputOptions, string optimizedOrder)
        {
            GetRenderer(outputOptions.Output).Render(outputOptions, optimizedOrder);
        }

        private static IRenderer GetRenderer(Output output)
        {
            return output switch
            {
                Output.Console => new Console.ConsoleRenderer(),
                Output.Json => new Json.JsonRenderer(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
