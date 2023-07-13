using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Downloader
{
    public class DownloadTaskInfo
    {
        public string Url { get; set; }

        public string SavePath { get; set; }

        public string Sha1 { get; set; }

        public int TryCount { get; set; }
    }
}
