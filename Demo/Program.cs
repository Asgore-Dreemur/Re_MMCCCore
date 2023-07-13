using System;
using MMCCCore.Functions.MC;
using MMCCCore.Tools;
using MMCCCore.DataClasses.Downloader;

namespace Program
{
    static class Program
    {
        public static void Main(string[] args)
        {
            FileDownloader downloader = new FileDownloader();
            downloader.OnProgressChanged += Downloader_OnProgressChanged;
            var result = downloader.DownloadFileTaskAsync(new DownloadTaskInfo
            {
                Url = "http://xyz.simpfun.vip:58386/test.bin",
                SavePath = "C:\\test.bin",
                TryCount = 4
            }).GetAwaiter().GetResult();
            Console.WriteLine(result);
        }
        
        private static void Downloader_OnProgressChanged(object sender, (long, long) e)
        {
            Console.WriteLine($"{e.Item1}/{e.Item2}");
        }
    }
}