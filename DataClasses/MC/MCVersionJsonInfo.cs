using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MMCCCore.DataClasses.MC
{
    public class MCVersionJsonInfo
    {
        [JsonProperty("arguments", NullValueHandling = NullValueHandling.Ignore)]
        public MCVersionJsonArgsInfo Arguments { get; set; }

        [JsonProperty("assetIndex")]
        public MCVersionJsonAssetIndexInfo AssetIndex { get; set; }

        [JsonProperty("assets", NullValueHandling = NullValueHandling.Ignore)]
        public string Assets { get; set; }

        [JsonProperty("downloads")]
        public Dictionary<string, MCFileInfo> Downloads { get; set; } = new Dictionary<string, MCFileInfo>();

        [JsonProperty("id")]
        public string VersionName { get; set; }

        [JsonProperty("javaVersion", NullValueHandling = NullValueHandling.Ignore)]
        public MCRecommentedJavaInfo JavaVersion { get; set; }

        [JsonProperty("libraries")]
        public List<MCVersionJsonLibraryInfo> Libraries { get; set; } = new List<MCVersionJsonLibraryInfo>();

        [JsonProperty("logging", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, MCVersionJsonLoggingInfo> Logging { get; set; } = new Dictionary<string, MCVersionJsonLoggingInfo>();

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }

        [JsonProperty("minecraftArguments")]
        public string MinecraftArguments { get; set; }

        [JsonProperty("minimumLauncherVersion")]
        public int MinimumLauncherVersion { get; set; }

        [JsonProperty("releaseTime")]
        public string ReleaseTime { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class MCVersionJsonAssetIndexInfo : MCFileInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }
    }

    public class MCVersionJsonArgsInfo
    {
        [JsonProperty("game")]
        public List<JToken> Game { get; set; } = new List<JToken>();

        [JsonProperty("jvm")]
        public List<JToken> Jvm { get; set; } = new List<JToken>();
    }

    public class MCRecommentedJavaInfo
    {
        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("majorVersion")]
        public string JavaVersion { get; set; }
    }

    public class MCFileInfo
    {
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("sha1", NullValueHandling = NullValueHandling.Ignore)]
        public string Sha1 { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public int Size { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    //Library

    public class MCVersionJsonLibraryInfo
    {
        [JsonProperty("downloads", NullValueHandling = NullValueHandling.Ignore)]
        public MCLibraryDownloadsInfo Downloads { get; set; }

        [JsonProperty("extract", NullValueHandling = NullValueHandling.Ignore)]
        public JObject Extract { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("natives", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Natives { get; set; }

        [JsonProperty("rules", NullValueHandling = NullValueHandling.Ignore)]
        public List<MCLibraryRuleInfo> Rules { get; set; }

        [JsonProperty("clientreq", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ClientReq { get; set; }
    }

    public class MCLibraryDownloadsInfo
    {
        [JsonProperty("artifact", NullValueHandling = NullValueHandling.Ignore)]
        public MCFileInfo Artifact { get; set; }

        [JsonProperty("classifiers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, MCFileInfo> Classifiters { get; set; }
    }

    public class MCLibraryRuleInfo
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("os")]
        public Dictionary<string, string> OS { get; set; } = new Dictionary<string, string>();
    }

    //log4j

    public class MCVersionJsonLoggingInfo
    {
        [JsonProperty("argument")]
        public string Argument { get; set; }

        [JsonProperty("file")]
        public MCFileInfo File { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
