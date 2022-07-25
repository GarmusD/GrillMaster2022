using GrillMaster.Client.AppConfiguration;
using GrillMaster.Client.DTO;
using GrillMaster.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrillMaster.Client.Renderer.Json
{
    internal class JsonRenderer : IRenderer
    {
        public void Render(OutputOptions outputOptions, string optimizedGrillJson)
        {
            try
            {
                File.WriteAllText(outputOptions.JsonOutputPath?.FullName!, optimizedGrillJson);
            }
            catch(IOException ioEx)
            {
                throw new GrillMasterIOException(ioEx.Message);
            }
        }
    }
}
