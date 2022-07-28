using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.CLI
{
    internal class ExitCodes
    {
        public const int JsonConfigMalformed = 1;
        public const int MenuGeneratorOffline = 2;
        public const int ApiServerOffline = 3;
        public const int AuthError = 4;
        public const int ApiError = 5;
        public const int AccessDeniedError = 6;
        public const int IOException = 7;
        public const int InvalidOptimizedJson = 8;
        public const int InvalidMenuOrderJson = 9;
    }
}
