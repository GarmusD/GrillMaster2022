using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.AppConfiguration
{
    public class OutputOptions
    {
        public Output Output { get; set; } = Output.Console;
        public FileInfo? JsonOutputPath { get; set; } = null;
    }
}
