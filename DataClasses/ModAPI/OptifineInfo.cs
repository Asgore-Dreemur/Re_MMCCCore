using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.ModAPI
{
    public class OptifineInfo
    {
        [JsonProperty("mcversion")]
        public string MCVersion { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("patch")]
        public string Patch { get; set; }

        [JsonProperty("__id")]
        public string ID { get; set; }

        public string GetDisplayName() => $"{this.MCVersion}_{this.Type}_{this.Patch}";
    }
}
