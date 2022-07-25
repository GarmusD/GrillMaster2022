using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Exceptions
{
    internal class GrillMasterFileAccessException : GrillMasterBaseException
    {
        public GrillMasterFileAccessException(string message) : base(message)
        {
            ExitCode = ExitCodes.AccessDeniedError;
        }
    }
}
