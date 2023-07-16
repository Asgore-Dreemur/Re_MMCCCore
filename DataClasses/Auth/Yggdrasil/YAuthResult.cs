using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMCCCore.DataClasses.Auth.Yggdrasil
{
    public class YAuthResult : YErrorInfo
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("clientToken")]
        public string ClientToken { get; set; }

        [JsonProperty("availableProfiles")]
        public List<YProfileOverInfo> AvailableProfiles { get; set; } = new List<YProfileOverInfo>();
    }

    public class YProfileOverInfo : YErrorInfo //外置登录 角色信息概览
    {
        [JsonProperty("id")]
        public string UUID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class YProfileInfo : YProfileOverInfo
    {
        [JsonProperty("properties")]
        public List<YProfilePropInfo> Properites { get; set; } = new List<YProfilePropInfo>();
    }

    public class YProfilePropInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class YErrorInfo
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
