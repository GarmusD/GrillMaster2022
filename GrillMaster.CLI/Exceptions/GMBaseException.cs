using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.CLI.Exceptions
{
    public class GMBaseException : Exception
    {
        public int ExitCode { get; protected set; }
        public GMBaseException(string message) : base(message) { }
    }
}
