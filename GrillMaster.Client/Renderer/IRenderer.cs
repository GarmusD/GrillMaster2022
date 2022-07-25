using GrillMaster.Client.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Renderer
{
    internal interface IRenderer
    {
        void Render(OutputOptions outputOptions, string optimizedGrillJson);
    }
}
