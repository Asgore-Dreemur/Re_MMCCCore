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
using System.Xml.Linq;

namespace MMCCCore.Functions.MC
{
    /// <summary>
    /// MC支持库方法封装
    /// </summary>
    public class MCLibraryManager
    {


        /// <summary>
        /// 从版本json中获取全部支持库信息
        /// </summary>
        /// <param name="info">版本json</param>
        /// <returns>支持库信息列表</returns>
        public static List<MCLibraryInfo> GetAllLibrariesFromJson(MCVersionJsonInfo info)
        {
            List<MCLibraryInfo> llist = new List<MCLibraryInfo>();
            foreach (var item in info.Libraries)
            {
                MCLibraryInfo linfo = new MCLibraryInfo { Name = item.Name, RawJson = item, isEnable = true };
                if (item.Downloads != null && item.Downloads.Artifact != null)
                {
                    linfo.Path = item.Downloads.Artifact.Path;
                    linfo.Url = item.Downloads.Artifact.Url;
                    linfo.Size = item.Downloads.Artifact.Size;
                    linfo.Sha1 = item.Downloads.Artifact.Sha1;
                    linfo.isNative = false;
                    if (linfo.RawJson.Rules != null) linfo.isEnable = IsEnableTheLibraryByRules(item);
                    llist.Add(linfo);
                }
                if (item.Downloads != null && item.Downloads.Classifiters != null)
                {
                    MCLibraryInfo cinfo = new MCLibraryInfo { Name = item.Name, RawJson = item, isEnable = true, isNative = true };
                    if (IsTheSystemNeedTheNative(item))
                    {
                        var file = item.Downloads.Classifiters[GetNativeKey(item)];
                        cinfo.Path = file.Path;
                        cinfo.Size = file.Size;
                        cinfo.Sha1 = file.Sha1;
                        cinfo.Url = file.Url;
                        if (item.Rules != null) cinfo.isEnable = IsEnableTheLibraryByRules(item);
                        llist.Add(cinfo);
                    }
                }
                if (item.Downloads == null)
                {
                    linfo.isEnable = item.ClientReq.HasValue ? item.ClientReq.Value : true;
                    if (!string.IsNullOrEmpty(item.Name)) linfo.Path = GetMavenPathFromName(item.Name);
                    linfo.Url = item.Url;
                    llist.Add(linfo);
                }
            }
            return llist;
        }





        /// <summary>
        /// 检查native是否被该系统需要
        /// </summary>
        /// <param name="info">支持库json</param>
        /// <returns>bool值,该系统是否需要这个native</returns>
        public static bool IsTheSystemNeedTheNative(MCVersionJsonLibraryInfo info)
        {
            string systemPlatform = OtherTools.GetSystemPlatformLikesMC();
            int arch = Environment.Is64BitOperatingSystem ? 64 : 32;
            if (!string.IsNullOrEmpty(systemPlatform))
            {
                if (info.Natives.ContainsKey(systemPlatform))
                {
                    string className = info.Natives[systemPlatform].Replace("${arch}", arch.ToString());
                    if (info.Downloads.Classifiters.ContainsKey(className)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查支持库的rules来判断该系统是否需要该支持库
        /// </summary>
        /// <param name="info">支持库json</param>
        /// <returns>bool值,该系统是否需要该支持库</returns>
        public static bool IsEnableTheLibraryByRules(MCVersionJsonLibraryInfo info)
        {
            Dictionary<string, bool> systems = new Dictionary<string, bool>
            {
                {"windows", false },
                {"linux", false },
                {"osx", false }
            };
            foreach (var item in info.Rules)
            {
                if (item.OS == default || !item.OS.ContainsKey("name"))
                {
                    if (item.Action == "allow") systems["windows"] = systems["linux"] = systems["osx"] = true;
                    else systems["windows"] = systems["linux"] = systems["osx"] = false;
                }
                else
                {
                    string targetName = item.OS["name"];
                    if (item.Action == "allow")
                    {
                        systems[targetName] = true;
                    }
                    else systems[targetName] = false;
                }
            }
            return systems[OtherTools.GetSystemPlatformLikesMC()];
        }

        /// <summary>
        /// 获取该系统在支持库Classifiters里的键
        /// </summary>
        /// <param name="info">支持库json</param>
        /// <returns>支持库Classifiters里的键</returns>
        public static string GetNativeKey(MCVersionJsonLibraryInfo info)
        {
            int arch = Environment.Is64BitOperatingSystem ? 64 : 32;
            return info.Natives[OtherTools.GetSystemPlatformLikesMC()].Replace("${arch}", arch.ToString());
        }

        /// <summary>
        /// 从maven文件的命名空间中拼接路径
        /// </summary>
        /// <param name="name">该maven文件的命名空间</param>
        /// <returns>拼接的路径</returns>
        public static string GetMavenPathFromName(string name)
        {
            var SplitArray = name.Split(':').ToList();
            string NamespacePath = SplitArray[0].Replace('.', Path.DirectorySeparatorChar);
            string PackageName = SplitArray[1];
            string PackageVersion = SplitArray[2];
            if (SplitArray.Count > 3)
            {
                var extarray = SplitArray.GetRange(3, SplitArray.Count - 3);
                string JarName = PackageVersion + $"-{string.Join("-", extarray)}";
                return NamespacePath + Path.DirectorySeparatorChar + PackageName
                    + Path.DirectorySeparatorChar + PackageVersion +
                    $"{Path.DirectorySeparatorChar}{PackageName}-{JarName}.jar";
            }
            return NamespacePath + Path.DirectorySeparatorChar + PackageName
                    + Path.DirectorySeparatorChar + PackageVersion +
                    $"{Path.DirectorySeparatorChar}{PackageName}-{PackageVersion}.jar";
        }
    }
}
