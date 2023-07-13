using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Downloader
{
    public class DownloadResult
    {
        public bool IsSuccess { get; set; }

        public Exception ErrorException { get; set; }

        public DownloadTaskInfo Task { get; set; }
    }
}
