using MMCCCore.DataClasses.Auth.Yggdrasil;
using MMCCCore.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace MMCCCore.Functions.Auth
{
    public class YggdrasilAuthenticator
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string YggdrasilServerUrl { get; set; }

        private string ClientToken { get; } = Guid.NewGuid().ToString("N");

        public YggdrasilAuthenticator() { }

        public YggdrasilAuthenticator(string username, string password, string yggdrasilServerUrl)
        {
            this.Username = username;
            this.Password = password;
            this.YggdrasilServerUrl = yggdrasilServerUrl;
        }

        /// <summary>
        /// 登录,将取用该实例的成员变量Username和Password进行登录,执行此方法前应先设定好这两个变量
        /// </summary>
        /// <param name="isRefresh">是否为刷新令牌</param>
        /// <returns>登录结果</returns>
        public async Task<YAuthResult> AuthenticateTaskAsync(bool isRefresh)
        {
            string operation = isRefresh ? "authenticate" : "refresh";
            string requestUrl = this.YggdrasilServerUrl.TrimEnd('/') + $"/authserver/{operation}";
            JObject keyValuePairs = new JObject();
            keyValuePairs["username"] = this.Username;
            keyValuePairs["password"] = this.Password;
            keyValuePairs["clientToken"] = this.ClientToken;
            var rcontext = await HttpTools.HttpPostTaskAsync(requestUrl, "application/json", JsonConvert.SerializeObject(keyValuePairs));
            return JsonConvert.DeserializeObject<YAuthResult>(await rcontext.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="uuid">角色uuid</param>
        /// <returns>角色信息</returns>
        public async Task<YProfileInfo> GetProfileInfoTaskAsync(string uuid)
        {
            string requestUrl = this.YggdrasilServerUrl.TrimEnd('/') + "/sessionserver/session/minecraft/profile/" + uuid;
            var rcontext = await HttpTools.HttpGetTaskAsync(requestUrl);
            if (rcontext.StatusCode != HttpStatusCode.OK) return null;
            return JsonConvert.DeserializeObject<YProfileInfo>(await rcontext.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 通过角色信息获取材质url
        /// </summary>
        /// <param name="info">角色信息</param>
        /// <returns>材质url,若没有材质则为null</returns>
        public async Task<string> GetTextureUrlTaskAsync(YProfileInfo info)
        {
            try
            {
                YProfilePropInfo textureProp = info.Properites.Find(i => i.Name == "textures");
                if (textureProp != default)
                {
                    string textureStr = Encoding.UTF8.GetString(Convert.FromBase64String(textureProp.Value));
                    if (!string.IsNullOrEmpty(textureStr))
                    {
                        YTextureInfo texturesInfo = JsonConvert.DeserializeObject<YTextureInfo>(textureStr);
                        if(texturesInfo != null)
                        {
                            if (texturesInfo.Textures.ContainsKey("SKIN"))
                            {
                                string url = texturesInfo.Textures["SKIN"]["url"].ToString();
                                return url;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// 登出,将取用该实例的成员变量Username和Password进行登出,执行此方法前应先设定好这两个变量
        /// </summary>
        /// <returns>bool,若登出成功则为true,否则为false(若失败可能是因为邮箱或密码错误,尝试次数太多等)</returns>
        public async Task<bool> SignOutTaskAsync()
        {
            string requestUrl = this.YggdrasilServerUrl.TrimEnd('/') + "/authserver/signout";
            JObject keyValuePairs = new JObject();
            keyValuePairs["username"] = this.Username;
            keyValuePairs["password"] = this.Password;
            var rcontext = await HttpTools.HttpPostTaskAsync(requestUrl, "application/json", JsonConvert.SerializeObject(keyValuePairs));
            if (rcontext.StatusCode == HttpStatusCode.NoContent) return true;
            return false;
        }
    }
}
