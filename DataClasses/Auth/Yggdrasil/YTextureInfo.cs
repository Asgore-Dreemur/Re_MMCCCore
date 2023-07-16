using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Auth.Yggdrasil
{
    public class YTextureInfo
    {
        [JsonProperty("profileId")]
        public string ProfileID { get; set; }

        [JsonProperty("profileName")]
        public string ProfileName { get; set; }

        [JsonProperty("textures")]
        public JObject Textures { get; set; }
    }
}
