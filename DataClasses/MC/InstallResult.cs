using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.MC
{
    public class InstallResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public Exception ErrorException { get; set; }

        public object OtherInfo { get; set; }
    }
}
