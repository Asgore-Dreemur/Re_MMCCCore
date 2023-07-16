using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MMCCCore.DataClasses.Auth.Microsoft
{
    public class XBLAuthRequestInfo
    {
        [JsonProperty("Properties")]
        public XBLAuthRequestProperitesInfo Properites { get; set; } = new XBLAuthRequestProperitesInfo();
        [JsonProperty("RelyingParty")]
        public string RelyingParty { get; set; } = "http://auth.xboxlive.com";
        [JsonProperty("TokenType")]
        public string TokenType { get; set; } = "JWT";
    }

    public class XBLAuthRequestProperitesInfo
    {
        [JsonProperty("AuthMethod")]
        public string AuthMethod { get; set; } = "RPS";

        [JsonProperty("SiteName")]
        public string SiteName { get; set; } = "user.auth.xboxlive.com";

        [JsonProperty("RpsTicket")]
        public string RpsTicket { get; set; } = "d=<access token>";
    }

    public class XBLAuthResponseInfo
    {
        [JsonProperty("IssueInstant")]
        public string IssueInstant { get; set; }

        [JsonProperty("NotAfter")]
        public string NotAfter { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("DisplayClaims")]
        public DisplayClaimsInfo DisplayClaims { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class DisplayClaimsInfo
    {
        [JsonProperty("xui")]
        public List<JObject> Xui { get; set; }
    }
}
