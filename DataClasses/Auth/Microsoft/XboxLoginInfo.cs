using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Auth.Microsoft
{
    public class XboxLoginResponse
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("roles")]
        public List<JToken> Roles { get; set; } = new List<JToken>();

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
