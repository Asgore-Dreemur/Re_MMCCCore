using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Auth.Microsoft
{
    public class AccountGamesModel
    {
        [JsonProperty("items")]
        public List<AccountItemModel> Items { get; set; } = new List<AccountItemModel>();

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("keyId")]
        public string KeyId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class AccountItemModel
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class PlayerProfileInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("skins")]
        public List<PlayerSkinInfo> Skins { get; set; } = new List<PlayerSkinInfo>();

        [JsonProperty("capes")]
        public JArray Capes { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class PlayerSkinInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }
    }
}
