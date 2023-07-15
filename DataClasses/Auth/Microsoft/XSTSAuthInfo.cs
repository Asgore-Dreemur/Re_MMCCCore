using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Auth.Microsoft
{
    public class XSTSAuthRequestInfo
    {
        [JsonProperty("Properties")]
        public XSTSAuthenticatePropertiesInfo Properties { get; set; } = new XSTSAuthenticatePropertiesInfo();

        [JsonProperty("RelyingParty")]
        public string RelyingParty { get; set; } = "rp://api.minecraftservices.com/";

        [JsonProperty("TokenType")]
        public string TokenType { get; set; } = "JWT";
    }

    public class XSTSAuthResponseInfo : XSTSAuthenticateErrorInfo
    {
        [JsonProperty("IssueInstant")]
        public string IssueInstant { get; set; }

        [JsonProperty("NotAfter")]
        public string NotAfter { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("DisplayClaims")]
        public DisplayClaimsInfo DisplayClaims { get; set; }
    }

    public class XSTSAuthenticateErrorInfo
    {
        [JsonProperty("Identity")]
        public string Identity { get; set; }

        [JsonProperty("XErr")]
        public string XErr { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Redirect")]
        public string Redirect { get; set; }
    }

    public class XSTSAuthenticatePropertiesInfo
    {
        [JsonProperty("SandboxId")]
        public string SandboxId { get; set; } = "RETAIL";

        [JsonProperty("UserTokens")]
        public List<string> UserTokens { get; set; } = new List<string>();
    }
}
