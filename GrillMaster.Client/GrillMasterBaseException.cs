using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client
{
    public class GrillMasterBaseException : Exception
    {
        public GrillMasterBaseException(string message) : base(message) { }
    }
}
