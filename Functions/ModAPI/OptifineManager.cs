using MMCCCore.DataClasses.MC;
using MMCCCore.DataClasses.ModAPI;
using MMCCCore.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Functions.ModAPI
{
    public static class OptifineManager
    {
        public static async Task<List<OptifineInfo>> GetAllOptifineFromMCVersion(string mcVersion)
        {
            string reqeustUrl = $"{APIManager.Bmclapi.Optifine}/{mcVersion}";
            var context = await HttpTools.HttpGetTaskAsync(reqeustUrl);
            var ilist = JsonConvert.DeserializeObject<List<OptifineInfo>>(await context.Content.ReadAsStringAsync());
            ilist.Reverse();
            return ilist;
        }

        public static async Task<List<OptifineInfo>> GetAllOptifineVersion()
        {
            string reqeustUrl = $"{APIManager.Bmclapi.Optifine}/versionList";
            var context = await HttpTools.HttpGetTaskAsync(reqeustUrl);
            return JsonConvert.DeserializeObject<List<OptifineInfo>>(await context.Content.ReadAsStringAsync());
        }
    }
}
