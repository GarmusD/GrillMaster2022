using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client
{
    public class GrillMasterOfflineException : GrillMasterBaseException
    {
        public GrillMasterOfflineException() : base("GrillMaster2022.API server is inaccessible. Start server or check settings and try again.")
        {
        }
    }
}
