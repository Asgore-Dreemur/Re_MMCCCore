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
    public static class MCVersionWrapper
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
        }
    }
}
