using MMCCCore.DataClasses.Downloader;
using MMCCCore.DataClasses.MC;
using MMCCCore.Tools;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Functions.MC
{
    public class MCLibraryDownloader
    {
        public event EventHandler<double> OnProgressChanged;

        /// <summary>
        /// 补全支持库并解压native
        /// </summary>
        /// <param name="gameRoot">游戏根目录</param>
        /// <param name="info">版本json</param>
        /// <returns>执行结果</returns>
        public async Task<InstallResult> CompleteLibrariesTaskAsync(string gameRoot, MCVersionJsonInfo info)
        {
            try
            {
                string librariesPath = Path.Combine(OtherTools.FormatPath(gameRoot), "libraries");
                OtherTools.CreateDirs(librariesPath);
                var llist = MCLibraryManager.GetAllLibrariesFromJson(info);
                Stack<DownloadTaskInfo> stack = new Stack<DownloadTaskInfo>();
                List<MCLibraryInfo> nativeLibraries = new List<MCLibraryInfo>();
                foreach (var item in llist)
                {
                    if (item.isEnable)
                    {
                        if (item.isNative) nativeLibraries.Add(item);
                        string path = OtherTools.FormatPath(Path.Combine(librariesPath, item.Path));
                        if (!string.IsNullOrEmpty(item.Url))
                        {
                            if (item.RawJson.Downloads != null)
                                stack.Push(new DownloadTaskInfo
                                {
                                    Url = item.Url,
                                    SavePath = path,
                                    Sha1 = string.IsNullOrEmpty(item.Sha1) ? string.Empty : item.Sha1,
                                    TryCount = DownloadConfigs.ErrorTryCount
                                });
                            else
                            {
                                if (File.Exists(path)) continue;
                                string url = item.Url.TrimEnd('/') + "/" + item.Path;
                                stack.Push(new DownloadTaskInfo
                                {
                                    Url = url,
                                    SavePath = path,
                                    Sha1 = string.IsNullOrEmpty(item.Sha1) ? string.Empty : item.Sha1,
                                    TryCount = DownloadConfigs.ErrorTryCount
                                });
                            }
                        }
                        else
                        {
                            if (File.Exists(path)) continue;
                            string url = $"https://{APIManager.Current.Libraries}/{item.Path}";
                            stack.Push(new DownloadTaskInfo
                            {
                                Url = url,
                                SavePath = path,
                                Sha1 = string.IsNullOrEmpty(item.Sha1) ? string.Empty : item.Sha1,
                                TryCount = DownloadConfigs.ErrorTryCount
                            });
                        }
                    }
                }
                MultiFileDownloader downloader = new MultiFileDownloader(stack, DownloadConfigs.ThreadCount);
                downloader.OnProgressChanged += Downloader_OnProgressChanged;
                downloader.StartDownload();
                var result = downloader.WaitForDownloadComplete();
                if (result.IsSuccess) return ExtractNatives(gameRoot, nativeLibraries).Result;
                else throw result.ErrorException;
            }
            catch (WebException e)
            {
                return new InstallResult
                {
                    IsSuccess = false,
                    ErrorException = e,
                    Message = "failed"
                };
            }
        }

        /// <summary>
        /// 解压natives
        /// </summary>
        /// <param name="gameRoot">游戏根目录</param>
        /// <param name="llist">native列表</param>
        /// <returns>执行结果</returns>
        public static async Task<InstallResult> ExtractNatives(string gameRoot, List<MCLibraryInfo> llist)
        {
            try
            {
                string librariesPath = Path.Combine(OtherTools.FormatPath(gameRoot), "libraries");
                string nativesPath = Path.Combine(OtherTools.FormatPath(gameRoot), "natives");
                OtherTools.CreateDirs(nativesPath);
                string sysPlatform = OtherTools.GetSystemPlatformLikesMC();
                string libExt = sysPlatform == "windows" ? ".dll" : 
                    sysPlatform == "linux" ? ".so" : 
                    sysPlatform == "osx" ? ".dylib" : string.Empty;
                foreach (var item in llist)
                {
                    string libraryPath = OtherTools.FormatPath(Path.Combine(librariesPath, item.Path));
                    ZipArchive archive = new ZipArchive(new FileStream(libraryPath, FileMode.Open));
                    var allLib = archive.Entries.ToList().FindAll(i =>
                    {
                        int index = i.Name.LastIndexOf('.');
                        if (index != -1)
                        {
                            return i.Name.Substring(index) == libExt;
                        }
                        return false;
                    });
                    if (allLib.Count > 0)
                    {
                        foreach (var entry in allLib)
                        {
                            string nativePath = Path.Combine(nativesPath, entry.Name);
                            var sha1File = archive.Entries.ToList().Find(i => i.Name == entry.Name + ".sha1");
                            if (sha1File != null)
                            {
                                string sha1 = new StreamReader(sha1File.Open()).ReadToEnd();
                                if (SHA1Tools.CompareSHA1(nativePath, sha1)) continue;
                            }
                            entry.ExtractToFile(nativePath, true);
                        }
                    }
                }
                return new InstallResult { IsSuccess = true, Message = "complete" };
            }
            catch (WebException e)
            {
                return new InstallResult
                {
                    IsSuccess = false,
                    ErrorException = e,
                    Message = "failed"
                };
            }
        }

        private void Downloader_OnProgressChanged(object sender, (int, int) e)
        {
            double status = Math.Round((double)e.Item1 / e.Item2, 2);
            OnProgressChanged?.Invoke(this, status);
        }
    }
}
