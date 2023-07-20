using MMCCCore.DataClasses;
using MMCCCore.DataClasses.MC;
using MMCCCore.DataClasses.ModAPI;
using MMCCCore.Tools;
using MMCCCore.DataClasses.Downloader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO.Compression;
using MMCCCore.Functions.MC;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace MMCCCore.Functions.ModAPI
{
    public class OptifineInstaller
    {
        public event EventHandler<(string, double)> ProgressChanged; //通知事件

        public string JavaPath { get; set; } //Java路径,必须指定
        public OptifineInfo InstallInfo { get; set; } //要安装的optifine信息
        public string GameRoot { get; set; } //游戏目录
        public string VersionName { get; set; } //安装版本名

        /// <summary>
        /// 执行安装程序
        /// </summary>
        /// <returns>安装结果</returns>
        public async Task<InstallResult> InstallTaskAsync()
        {
            try
            {
                //检查参数情况
                if (string.IsNullOrEmpty(JavaPath) ||
                    string.IsNullOrEmpty(GameRoot) ||
                    string.IsNullOrEmpty(VersionName) ||
                    InstallInfo == null) throw new Exception("Null varible");
                //检查版本是否存在
                if (MCVersionManager.VersionExists(GameRoot, VersionName))
                    throw new Exception(LocalLang.Current.ExistsVersionError);
                //下载Optifine
                ProgressChanged?.Invoke(this, (LocalLang.Current.Optifine_Downloading, 0.2));
                var result = await DownloadOptifineJar();
                string optifineJarPath = (string)result.OtherInfo; //获取传出的optifine路径
                if (!result.IsSuccess) throw result.ErrorException;
                //写入文件
                ProgressChanged?.Invoke(this, (LocalLang.Current.Optifine_WritingFile, 0.4));
                result = await WriteFiles(optifineJarPath);
                if (!result.IsSuccess) throw result.ErrorException;
                //运行安装器
                ProgressChanged?.Invoke(this, (LocalLang.Current.Optifine_RunningProcess, 0.8));
                result = await RunProcess(optifineJarPath, (string)result.OtherInfo);
                return result;
            }
            catch(Exception e)
            {
                return new InstallResult { IsSuccess = false, ErrorException = e };
            }
        }

        /// <summary>
        /// 安装第一步,下载optifine安装包
        /// </summary>
        /// <returns>执行结果</returns>
        private async Task<InstallResult> DownloadOptifineJar()
        {
            try
            {
                string optifinePath = Path.Combine(Path.GetTempPath(), "optifine.jar");
                string url = $"{APIManager.Bmclapi.Optifine}/{InstallInfo.MCVersion}/{InstallInfo.Type}/{InstallInfo.Patch}";
                var dresult = FileDownloader.DownloadFile(new DownloadTaskInfo { Url = url, SavePath = optifinePath, TryCount = DownloadConfigs.ErrorTryCount });
                if (!dresult.IsSuccess) throw dresult.ErrorException;
                return new InstallResult { IsSuccess = true, OtherInfo = optifinePath };
            }
            catch (Exception e)
            {
                return new InstallResult { IsSuccess = false, ErrorException = e };
            }
        }

        /// <summary>
        /// 安装第二步,写入文件
        /// </summary>
        /// <param name="optifineJarPath">optifine安装包路径</param>
        /// <returns>执行结果</returns>
        private async Task<InstallResult> WriteFiles(string optifineJarPath)
        {
            try
            {
                ZipArchive archive = new ZipArchive(new FileStream(optifineJarPath, FileMode.Open));
                var lentry = archive.GetEntry("launchwrapper-of.txt");
                string launchWrapperVersion = "", launchWrapperName = "", launchWrapperLibPath = "";
                if (lentry != null)
                {
                    launchWrapperVersion = new StreamReader(lentry.Open()).ReadToEnd();
                }
                else launchWrapperVersion = "1.12";
                if(launchWrapperVersion != "1.12")
                {
                    var jentry = archive.GetEntry($"launchwrapper-of-{launchWrapperVersion}.jar");
                    launchWrapperName = $"optifine:launchwrapper-of:{launchWrapperVersion}";
                    launchWrapperLibPath = Path.Combine(GameRoot, "libraries", MCLibraryManager.GetMavenPathFromName(launchWrapperName));
                    FileInfo finfo = new FileInfo(launchWrapperLibPath);
                    if (!finfo.Directory.Exists) finfo.Directory.Create();
                    if(!finfo.Exists)jentry.ExtractToFile(launchWrapperLibPath);
                }
                else
                {
                    launchWrapperName = "net.minecraft:launchwrapper:1.12";
                    launchWrapperLibPath = Path.Combine(GameRoot, "libraries", MCLibraryManager.GetMavenPathFromName(launchWrapperName));
                    if (!File.Exists(launchWrapperLibPath))
                    {
                        var dresult = FileDownloader.DownloadFile(new DownloadTaskInfo
                        {
                            Url = $"https://{APIManager.Current.Libraries}/{MCLibraryManager.GetMavenPathFromName(launchWrapperName)}",
                            SavePath = launchWrapperLibPath,
                            TryCount = DownloadConfigs.ErrorTryCount
                        });
                        if (!dresult.IsSuccess) throw dresult.ErrorException;
                    }
                }
                string optifineName = $"optifine:Optifine:{InstallInfo.GetDisplayName()}";
                string optifineLibPath = Path.Combine(GameRoot, "libraries", MCLibraryManager.GetMavenPathFromName(optifineName));
                FileInfo oinfo = new FileInfo(optifineLibPath);
                if (!oinfo.Directory.Exists) oinfo.Directory.Create();
                File.Copy(optifineJarPath, optifineLibPath, true);
                MCVersionJsonInfo info = new MCVersionJsonInfo
                {
                    Arguments = new MCVersionJsonArgsInfo
                    {
                        Game = new List<JToken>
                        {
                            "--tweakClass",
                            "optifine.OptiFineTweaker"
                        }
                    },
                    VersionName = VersionName,
                    InheritsFrom = InstallInfo.MCVersion,
                    Time = DateTime.Now.ToString("O"),
                    ReleaseTime = DateTime.Now.ToString("O"),
                    Type = "release",
                    MainClass = "net.minecraft.launchwrapper.Launch",
                    Libraries = new List<MCVersionJsonLibraryInfo>
                    {
                        new MCVersionJsonLibraryInfo{Name = optifineName},
                        new MCVersionJsonLibraryInfo{Name = launchWrapperName}
                    }
                };
                string versionJsonPath = Path.Combine(GameRoot, "versions", VersionName, VersionName + ".json");
                FileInfo vinfo = new FileInfo(versionJsonPath);
                if (!vinfo.Directory.Exists) vinfo.Directory.Create();
                File.WriteAllText(versionJsonPath, JsonConvert.SerializeObject(info, Formatting.Indented));
                return new InstallResult { IsSuccess = true, OtherInfo = launchWrapperLibPath };
            }
            catch (ThreadAbortException e) 
            { 
                return new InstallResult { IsSuccess = false, ErrorException = e }; 
            };
        }

        /// <summary>
        /// 安装最后一步,运行安装器
        /// </summary>
        /// <param name="optifineJarPath">optifine安装包路径</param>
        /// <param name="launchWrapperPath">launchWrapper jar路径</param>
        /// <returns>执行结果</returns>
        private async Task<InstallResult> RunProcess(string optifineJarPath, string launchWrapperPath)
        {
            try
            {
                string rawJarPath = Path.Combine(GameRoot, "versions", InstallInfo.MCVersion, InstallInfo.MCVersion + ".jar");
                if (File.Exists(rawJarPath))
                {
                    string errorMessage = "";
                    Process process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = JavaPath,
                            Arguments = $"-cp \"{optifineJarPath}\"" +
                                $" optifine.Patcher" +
                                $" \"{rawJarPath}\"" +
                                $" \"{optifineJarPath}\"" +
                                $" \"{launchWrapperPath}\"",
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    process.BeginErrorReadLine();
                    process.ErrorDataReceived += (_, e) => errorMessage += e.Data;
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                        throw new Exception(LocalLang.Current.Optifine_ProcessError.Replace("{0}", errorMessage));
                    else return new InstallResult { IsSuccess = true };
                }
                else
                {
                    throw new Exception(LocalLang.Current.CannotFindRawJar);
                }
            }
            catch(Exception e)
            {
                return new InstallResult { IsSuccess = false, ErrorException = e };
            }
        }
    }
}
