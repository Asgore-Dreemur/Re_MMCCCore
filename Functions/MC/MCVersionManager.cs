using MMCCCore.DataClasses.MC;
using MMCCCore.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Functions.MC
{
    /// <summary>
    /// MC版本一些方法的封装
    /// </summary>
    public static class MCVersionManager
    {
        /// <summary>
        /// 获取所有的MC版本
        /// </summary>
        /// <returns>MCVersionsInfo,MC版本数据类</returns>
        public static MCVersionsInfo GetAllMCVersions()
        {
            return JsonConvert.DeserializeObject<MCVersionsInfo>(
                HttpTools.HttpGetTaskAsync(APIManager.Current.VersionManifest)
                .Result.Content.ReadAsStringAsync().Result);
        }

        public static MCModAPIType LocalGetMCAPIType(string gameRoot, string id)
        {
            string versionPath = Path.Combine(gameRoot, "versions", id);
            if (Directory.Exists(versionPath))
            {
                string versionJsonPath = Path.Combine(versionPath, id + ".json");
                if (File.Exists(versionJsonPath))
                {
                    string fileContext = File.ReadAllText(versionJsonPath);
                    MCVersionJsonInfo info = JsonConvert.DeserializeObject<MCVersionJsonInfo>(fileContext);
                    if(info != null)
                    {

                    }
                }
            }
            return MCModAPIType.Raw;
        }

        /// <summary>
        /// 检查给定版本是否存在
        /// </summary>
        /// <param name="gameRoot">游戏根目录</param>
        /// <param name="versionName">版本名</param>
        /// <returns></returns>
        public static bool VersionExists(string gameRoot, string versionName)
        {
            try
            {
                string versionPath = Path.Combine(gameRoot, "versions", versionName);
                if (Directory.Exists(versionPath))
                {
                    string jsonPath = Path.Combine(versionPath, versionName + ".json");
                    if (File.Exists(jsonPath))
                    {
                        var info = JsonConvert.DeserializeObject<MCVersionJsonInfo>(File.ReadAllText(jsonPath));
                        if (info != null)
                        {
                            if (!string.IsNullOrEmpty(info.VersionName)) return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception) { return false; }
        }
    }
}
