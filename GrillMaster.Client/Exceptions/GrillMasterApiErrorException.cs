using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Exceptions
{
    internal class GrillMasterApiErrorException : GrillMasterBaseException
    {
        public GrillMasterApiErrorException() : base("GrillMaster2002 Optimizer API error.")
        {
            ExitCode = ExitCodes.ApiError;
        }
    }
}
