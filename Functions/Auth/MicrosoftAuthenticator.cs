using MMCCCore.DataClasses;
using MMCCCore.DataClasses.Auth.Microsoft;
using MMCCCore.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Functions.Auth
{
    public class MicrosoftAuthenticator
    {
        private string ClientID { get; set; } = "edab33c8-7489-4eb1-a5e3-dc573898b6b0";

        public MicrosoftAuthenticator() { }

        public MicrosoftAuthenticator(string clientID) => this.ClientID = clientID; //使用自己的客户端ID

        public event EventHandler<(string, double)> OnProgressChanged; //自动化验证通知进度

        /// <summary>
        /// 自动化验证，在完成设备代码流验证后调用此方法
        /// </summary>
        /// <param name="info">设备代码流验证结果</param>
        /// <returns>微软账号实例</returns>
        public async Task<MicrosoftAccountInfo> AuthenticateTaskAsync(OAuth2Result info)
        {
            try
            {
                OnProgressChanged?.Invoke(this, (LocalLang.Current.Microsoft_XBLAuthing, 0.20));
                //XBL
                var xblAuthResult = await XBLAuthTaskAsync(info);
                if (xblAuthResult == null || !string.IsNullOrEmpty(xblAuthResult?.Error)) 
                    throw new Exception($"Error:{xblAuthResult.Error}");
                //XSTS
                OnProgressChanged?.Invoke(this, (LocalLang.Current.Microsoft_XSTSAuthing, 0.40));
                var xstsAuthResult = await XSTSAuthTaskAsync(xblAuthResult);
                if (xstsAuthResult == null || !string.IsNullOrEmpty(xstsAuthResult?.XErr))
                    throw new Exception($"Error:{xblAuthResult.Error}");
                //Xbox login
                OnProgressChanged?.Invoke(this, (LocalLang.Current.Microsoft_XboxLoggingIn, 0.60));
                var loginResult = await LoginMinecraftWithXbox(xstsAuthResult);
                if (loginResult == null)
                    throw new Exception(LocalLang.Current.Microsoft_XboxLoginError);
                //Check
                OnProgressChanged?.Invoke(this, (LocalLang.Current.Microsoft_CheckingHasMinecraft, 0.70));
                bool haveMC = await IsTheAccountHasMinecraft(loginResult.AccessToken);
                if (!haveMC) throw new Exception(LocalLang.Current.Microsoft_HasNoMinecraft);
                //Get profile
                OnProgressChanged?.Invoke(this, (LocalLang.Current.Microsoft_GettingPlayerProfile, 0.80));
                var profile = await GetPlayerProfile(loginResult.AccessToken);
                if (profile == null || !string.IsNullOrEmpty(profile?.Error))
                    throw new Exception(LocalLang.Current.Microsoft_ProfileGettingError);
                return new MicrosoftAccountInfo
                {
                    AccessToken = loginResult.AccessToken,
                    UUID = profile.ID,
                    Name = profile.Name,
                    RefreshToken = info.RefreshToken,
                    Profile = profile
                };
            }
            catch(Exception e)
            {
                return new MicrosoftAccountInfo { ErrorException = e };
            }
        }

        /// <summary>
        /// 开始设备代码流验证，进行微软登录时先执行此方法
        /// </summary>
        /// <returns>验证结果</returns>
        public async Task<DeviceFlowResponse> StartDeviceFlowTaskAsync()
        {
            Dictionary<string, string> requestDict = new Dictionary<string, string>
            {
                {"client_id", this.ClientID },
                {"scope", "XboxLive.signin offline_access" }
            };
            string requestUrl = $"https://login.microsoftonline.com/consumers/oauth2/v2.0/devicecode?mkt={LocalLang.CurrentLang}";
            var result = await HttpTools.HttpPostTaskAsync(requestUrl, requestDict);
            return JsonConvert.DeserializeObject<DeviceFlowResponse>(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 轮询直到用户完成设备代码流请求
        /// </summary>
        /// <param name="deviceFlowResult">设备代码流验证结果</param>
        /// <returns>轮询结果,若失败则返回null</returns>
        public async Task<OAuth2Result> WaitForUserCompleteRequest(DeviceFlowResponse deviceFlowResult)
        {
            int whileTime = 0;
            while (whileTime < deviceFlowResult.ExpiresIn)
            {
                Dictionary<string, string> requestDict = new Dictionary<string, string>
                {
                    {"grant_type", "urn:ietf:params:oauth:grant-type:device_code" },
                    {"client_id", this.ClientID },
                    {"device_code", deviceFlowResult.DeviceCode }
                };
                string requestUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
                var result = await HttpTools.HttpPostTaskAsync(requestUrl, requestDict);
                var aresult = JsonConvert.DeserializeObject<OAuth2Result>(await result.Content.ReadAsStringAsync());
                if (result.IsSuccessStatusCode) return aresult;
                else if (aresult.Error != "authorization_pending") return null;
                Thread.Sleep(deviceFlowResult.InterVal * 1000);
                whileTime += deviceFlowResult.InterVal;
            }
            return null;
        }

        /// <summary>
        /// XBL验证,完成设备代码流验证后执行此方法
        /// </summary>
        /// <param name="info">设备代码流验证结果</param>
        /// <returns>XBL验证结果</returns>
        private async Task<XBLAuthResponseInfo> XBLAuthTaskAsync(OAuth2Result info)
        {
            var model = new XBLAuthRequestInfo
            {
                Properites = new XBLAuthRequestProperitesInfo
                {
                    AuthMethod = "RPS",
                    SiteName = "user.auth.xboxlive.com",
                    RpsTicket = $"d={info.AccessToken}"
                },
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT"
            };
            var response = await HttpTools.HttpPostTaskAsync("https://user.auth.xboxlive.com/user/authenticate",
                "application/json", JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<XBLAuthResponseInfo>(await response.Content.ReadAsStringAsync());
        } 

        /// <summary>
        /// XSTS验证,在完成XBL验证后执行此方法
        /// </summary>
        /// <param name="info">XBL验证结果</param>
        /// <returns>XSTS验证结果</returns>
        private async Task<XSTSAuthResponseInfo> XSTSAuthTaskAsync(XBLAuthResponseInfo info)
        {
            XSTSAuthRequestInfo model = new XSTSAuthRequestInfo()
            {
                Properties = new XSTSAuthenticatePropertiesInfo
                {
                    SandboxId = "RETAIL",
                    UserTokens = new List<string>() { info.Token }
                },
                RelyingParty = "rp://api.minecraftservices.com/",
                TokenType = "JWT"
            };
            var response = await HttpTools.HttpPostTaskAsync("https://xsts.auth.xboxlive.com/xsts/authorize",
                "application/json", JsonConvert.SerializeObject(model));
            return JsonConvert.DeserializeObject<XSTSAuthResponseInfo>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 登录Xbox,XSTS验证完成后执行此方法
        /// </summary>
        /// <param name="model">XSTS验证结果</param>
        /// <returns></returns>
        private async Task<XboxLoginResponse> LoginMinecraftWithXbox(XSTSAuthResponseInfo model)
        {
            JObject keyValuePairs = new JObject();
            keyValuePairs["identityToken"] = $"XBL3.0 x={model.DisplayClaims.Xui[0]["uhs"]};{model.Token}";
            var result = await HttpTools.HttpPostTaskAsync("https://api.minecraftservices.com/authentication/login_with_xbox",
                "application/json", JsonConvert.SerializeObject(keyValuePairs));
            return JsonConvert.DeserializeObject<XboxLoginResponse>(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 检查账号是否购买了Minecraft,登录Xbox后可以执行此方法验证
        /// </summary>
        /// <param name="authToken">登录Xbox后获得的AccessToken</param>
        /// <returns>该账号是否拥有Minecraft</returns>
        private async Task<bool> IsTheAccountHasMinecraft(string authToken)
        {
            var result = await HttpTools.HttpGetTaskAsync("https://api.minecraftservices.com/entitlements/mcstore",
                AuthTuple: new Tuple<string, string>("Bearer", authToken));
            var info = JsonConvert.DeserializeObject<AccountGamesModel>(await result.Content.ReadAsStringAsync());
            if (info.Error != null) return false;
            else
            {
                var game = info.Items.Find(i => i.Name == "game_minecraft");
                if (game == null) return false;
                else return true;
            }
        }

        /// <summary>
        /// 获取MC账号信息,登录Xbox后执行此方法获取用户信息
        /// </summary>
        /// <param name="AuthToken">登录Xbox后取得的AccessToken</param>
        /// <returns>用户信息</returns>
        private async Task<PlayerProfileInfo> GetPlayerProfile(string AuthToken)
        {
            var result = await HttpTools.HttpGetTaskAsync("https://api.minecraftservices.com/minecraft/profile",
                AuthTuple: new Tuple<string, string>("Bearer", AuthToken));
            var info = JsonConvert.DeserializeObject<PlayerProfileInfo>(await result.Content.ReadAsStringAsync());
            return info;
        }

        /// <summary>
        /// 刷新登录令牌，在刷新令牌时应第一调用此方法
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>用户验证结果</returns>
        public async Task<OAuth2Result> RefreshTokenTaskAsync(string refreshToken)
        {
            Dictionary<string, string> requestDict = new Dictionary<string, string>
            {
                {"client_id", this.ClientID },
                {"refresh_token",  refreshToken},
                {"grant_type", "refresh_token" }
            };
            var authCodePostRes = await HttpTools.HttpPostTaskAsync($"https://login.live.com/oauth20_token.srf", requestDict);
            return JsonConvert.DeserializeObject<OAuth2Result>(await authCodePostRes.Content.ReadAsStringAsync());
        }
    }
}
