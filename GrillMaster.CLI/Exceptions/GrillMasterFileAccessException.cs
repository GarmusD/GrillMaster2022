using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.CLI.Exceptions
{
    internal class GrillMasterFileAccessException : GMBaseException
    {
        public GrillMasterFileAccessException(string message) : base(message)
        {
            ExitCode = ExitCodes.AccessDeniedError;
        }
    }
}
