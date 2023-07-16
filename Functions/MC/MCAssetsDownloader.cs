using MMCCCore.DataClasses.Downloader;
using MMCCCore.DataClasses.MC;
using MMCCCore.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Functions.MC
{
    public class MCAssetsDownloader
    {
        public event EventHandler<double> OnProgressChanged;


        /// <summary>
        /// 下载/补全资源文件
        /// </summary>
        /// <param name="gameRoot">游戏根目录</param>
        /// <param name="info">版本json</param>
        /// <returns>执行结果</returns>
        public async Task<InstallResult> DownloadAssets(string gameRoot, MCVersionJsonInfo info)
        {
            try
            {
                string indexPath = Path.Combine(gameRoot, "assets", "indexes");
                string objectsPath = Path.Combine(gameRoot, "assets", "objects");
                OtherTools.CreateDirs(indexPath);
                OtherTools.CreateDirs(objectsPath);
                string indexContext = "";
                string indexFilePath = Path.Combine(indexPath, info.AssetIndex.ID + ".json");
                if (SHA1Tools.CompareSHA1(indexFilePath, info.AssetIndex.Sha1)) //如果已经存在且sha1一致
                {
                    indexContext = File.ReadAllText(indexFilePath); //直接读取
                }
                else
                {
                    string indexUrl = info.AssetIndex.Url;
                    indexContext = await (await HttpTools.HttpGetTaskAsync(indexUrl)).Content.ReadAsStringAsync(); //下载
                    File.WriteAllText(indexFilePath, indexContext);
                }
                JObject indexObj = JObject.Parse(indexContext);
                Stack<DownloadTaskInfo> stack = new Stack<DownloadTaskInfo>();
                foreach(var item in indexObj["objects"].Values())
                {
                    string assetHash = item["hash"].ToString();
                    string assetHash2B = assetHash.Substring(0, 2);
                    string assetUrl = $"https://{APIManager.Current.Asset}/{assetHash2B}/{assetHash}";
                    OtherTools.CreateDirs(Path.Combine(objectsPath, assetHash2B));
                    string assetPath = Path.Combine(objectsPath, assetHash2B, assetHash);
                    var dinfo = new DownloadTaskInfo
                    {
                        SavePath = assetPath,
                        Url = assetUrl,
                        Sha1 = assetHash,
                        TryCount = DownloadConfigs.ErrorTryCount
                    };
                    stack.Push(dinfo);
                }
                MultiFileDownloader downloader = new MultiFileDownloader(stack, DownloadConfigs.ThreadCount);
                downloader.OnProgressChanged += Downloader_OnProgressChanged;
                downloader.StartDownload();
                var result = downloader.WaitForDownloadComplete();
                if (result.IsSuccess) return new InstallResult { IsSuccess = true, Message = "Complete." };
                throw result.ErrorException;
            }
            catch(Exception e)
            {
                return new InstallResult
                {
                    IsSuccess = false,
                    ErrorException = e,
                    Message = "failed."
                };
            };
        }

        private void Downloader_OnProgressChanged(object sender, (int, int) e)
        {
            double status = Math.Round((double)e.Item1 / e.Item2, 2);
            OnProgressChanged?.Invoke(this, status);
        }
    }
}
