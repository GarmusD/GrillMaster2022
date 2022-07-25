using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Exceptions
{
    internal class GrillMasterInvalidJsonException : GrillMasterBaseException
    {
        public GrillMasterInvalidJsonException() : base("Invalid GrillMaster optimized json format.")
        {
            ExitCode = ExitCodes.InvalidOptimizedJson;
        }
    }
}
