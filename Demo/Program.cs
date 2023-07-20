using System;
using MMCCCore.Functions.MC;
using MMCCCore.Tools;
using MMCCCore.DataClasses.Downloader;
using MMCCCore.DataClasses.MC;
using Newtonsoft.Json;
using MMCCCore.DataClasses;
using MMCCCore.Functions.Auth;
using MMCCCore.Functions.ModAPI;

namespace Program
{
    static class Program
    {
        public static void Main(string[] args)
        {
            DownloadConfigs.ErrorTryCount = 4;
            DownloadConfigs.ThreadCount = 4;
            LocalLang.Current = LocalLang.ZH_CN;
            LocalLang.CurrentLang = "zh-CN";
            var info = OptifineManager.GetAllOptifineFromMCVersion("1.16.5").Result[0];
            OptifineInstaller installer = new OptifineInstaller()
            {
                GameRoot = "F:\\MCGame\\.minecraft",
                JavaPath = "java",
                InstallInfo = info,
                VersionName = "1.16.5optifineTest2"
            };
            installer.ProgressChanged += Installer_ProgressChanged;
            var result = installer.InstallTaskAsync().Result;
            Console.WriteLine(result);
        }

        private static void Installer_ProgressChanged(object sender, (string, double) e)
        {
            Console.WriteLine($"status:{e.Item2}({e.Item1})");
        }

        private static void Authenticator_OnProgressChanged(object sender, (string, double) e)
        {
            Console.WriteLine($"status:{e.Item2}({e.Item1})");
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