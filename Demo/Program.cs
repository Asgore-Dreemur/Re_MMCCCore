using System;
using MMCCCore.Functions.MC;
using MMCCCore.Tools;
using MMCCCore.DataClasses.Downloader;
using MMCCCore.DataClasses.MC;
using Newtonsoft.Json;
using MMCCCore.DataClasses;
using MMCCCore.Functions.Auth;

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
            MicrosoftAuthenticator authenticator = new MicrosoftAuthenticator();
            authenticator.OnProgressChanged += Authenticator_OnProgressChanged;
            var dfresult = authenticator.StartDeviceFlowTaskAsync().GetAwaiter().GetResult();
            Console.WriteLine($"code:{dfresult.UserCode} url:{dfresult.VerificationUri}  message:{dfresult.Message}");
            var oauthresult = authenticator.WaitForUserCompleteRequest(dfresult).GetAwaiter().GetResult();
            if (oauthresult != null)
            {
                var account = authenticator.AuthenticateTaskAsync(oauthresult).GetAwaiter().GetResult();
                Console.WriteLine(account);
            }
            else Console.WriteLine("Error");
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