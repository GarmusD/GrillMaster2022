using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.Exceptions
{
    public class GrillMasterBaseException : Exception
    {
        public int ExitCode { get; protected set; }
        public GrillMasterBaseException(string message) : base(message) { }
    }
}
