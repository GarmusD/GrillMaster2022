using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Exceptions
{
    internal class GrillMasterIOException : GrillMasterBaseException
    {
        public GrillMasterIOException(string message) : base(message)
        {
            ExitCode = ExitCodes.IOException;
        }
    }
}
