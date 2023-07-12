using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMCCCore.DataClasses.MC
{
    public class MCVersionsInfo
    {
        [JsonProperty("latest")]
        public MCVersionLatestInfo Latest { get; set; }

        [JsonProperty("versions")]
        public List<MCVersionInfo> Versions { get; set; } = new List<MCVersionInfo>();
    }

    public class MCVersionLatestInfo
    {
        [JsonProperty("release")]
        public string Release { get; set; }

        [JsonProperty("snapshot")]
        public string Snapshot { get; set; }
    }

    public class MCVersionInfo
    {
        [JsonProperty("id")]
        public string VersionName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string JsonUrl { get; set; }

        [JsonProperty("releaseTime")]
        public string ReleaseTime { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }
    }

    /// <summary>
    /// 用于表示本地MC信息的类
    /// </summary>
    public class LocalMCVersionInfo
    {
        public string Name { get; set; } //版本名

        public DateTime Time { get; set; } //安装时间

        public MCModAPIType APIType { get; set; } //ModAPI类型

        public string GameType { get; set; } //游戏类型(正式版/快照版/早期版本/远古版)

        public string ParentID { get; set; } //继承游戏版本
    }


    /// <summary>
    /// 表示ModAPI类型的枚举类
    /// </summary>
    public enum MCModAPIType
    {
        Raw,
        Optifine,
        Forge,
        Fabric,
        Liteloader
    }
}
