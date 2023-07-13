using System;
using MMCCCore.Functions.MC;
using MMCCCore.Tools;
using MMCCCore.DataClasses.Downloader;
using MMCCCore.DataClasses.MC;
using Newtonsoft.Json;

namespace Program
{
    static class Program
    {
        public static void Main(string[] args)
        {
            MCVersionJsonInfo info = JsonConvert.DeserializeObject<MCVersionJsonInfo>(File.ReadAllText("F:\\MCGame\\.minecraft\\versions\\1.19.2-Fabric 0.14.21\\1.19.2-Fabric 0.14.21.json"));
            MCAssetsManager manager = new MCAssetsManager();
            manager.OnProgressChanged += Manager_OnProgressChanged;
            var result = manager.DownloadAssets("F:\\MCGame\\.minecraft", info, 32).GetAwaiter().GetResult();
            Console.WriteLine(result);
        }

        private static void Manager_OnProgressChanged(object sender, double e)
        {
            Console.WriteLine($"进度:{e * 100}");
        }

        private static void Downloader_OnProgressChanged(object sender, (long, long) e)
        {
            Console.WriteLine($"{e.Item1}/{e.Item2}");
        }
    }
}