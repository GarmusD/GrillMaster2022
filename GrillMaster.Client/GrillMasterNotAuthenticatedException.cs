using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client
{
    public class GrillMasterNotAuthenticatedException : GrillMasterBaseException
    {
        public GrillMasterNotAuthenticatedException() : base("Authentication error. Set username and password in the 'appsettings.json' or set them as commandline parameters.")
        {
        }
    }
}
