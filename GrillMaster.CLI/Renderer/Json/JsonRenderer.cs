using GrillMaster.CLI.AppConfiguration;
using GrillMaster.CLI.Exceptions;
using GrillMaster.Data.DTO;

namespace GrillMaster.CLI.Renderer.Json
{
    internal class JsonRenderer : IRenderer
    {
        public void Render(OutputOptions outputOptions, OptimizedOrder optimizedOrder)
        {
            try
            {
                string outStr = System.Text.Json.JsonSerializer.Serialize(optimizedOrder, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));
                File.WriteAllText(outputOptions.JsonOutputPath?.FullName!, outStr);
            }
            catch(IOException ioEx)
            {
                throw new GMIOException(ioEx.Message);
            }
        }
    }
}
