using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client
{
    public class GrillMasterInvalidJsonException : GrillMasterBaseException
    {
        public GrillMasterInvalidJsonException() : base("Invalid GrillMaster optimized json format.")
        {
        }
    }
}
