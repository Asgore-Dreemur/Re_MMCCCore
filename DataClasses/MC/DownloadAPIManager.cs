using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.MC
{
    public class DownloadAPIInfo
    {
        public string VersionManifest { get; set; }

        public string Launcher { get; set; }

        public string LauncherMeta { get; set; }

        public string Asset { get; set; }

        public string Libraries { get; set; }

        public string Forge { get; set; }

        public string Liteloader { get; set; }

        public string AuthLib { get; set; }

        public string FabricMeta { get; set; }

        public string FabricMaven { get; set; }
    }

    public static class APIManager
    {
        public static DownloadAPIInfo Raw { get; } = new DownloadAPIInfo
        {
            VersionManifest = "http://launchermeta.mojang.com/mc/game/version_manifest_v2.json",
            Launcher = "launcher.mojang.com",
            LauncherMeta = "launchermeta.mojang.com",
            Asset = "resources.download.minecraft.net",
            Libraries = "libraries.minecraft.net",
            Forge = "files.minecraftforge.net",
            Liteloader = "http://dl.liteloader.com/versions/versions.json",
            AuthLib = "authlib-injector.yushi.moe",
            FabricMaven = "maven.fabricmc.net",
            FabricMeta = "meta.fabricmc.net"
        };

        public static DownloadAPIInfo Bmclapi { get; } = new DownloadAPIInfo
        {
            VersionManifest = "https://bmclapi2.bangbang93.com/mc/game/version_manifest_v2.json",
            Launcher = "bmclapi2.bangbang93.com",
            LauncherMeta = "bmclapi2.bangbang93.com",
            Asset = "bmclapi2.bangbang93.com/assets",
            Libraries = "bmclapi2.bangbang93.com/maven",
            Forge = "bmclapi2.bangbang93.com/maven",
            Liteloader = "https://bmclapi.bangbang93.com/maven/com/mumfrey/liteloader/versions.json",
            AuthLib = "bmclapi2.bangbang93.com/mirrors/authlib-injector",
            FabricMaven = "bmclapi2.bangbang93.com/maven",
            FabricMeta = "bmclapi2.bangbang93.com/fabric-meta"
        };

        public static DownloadAPIInfo Mcbbs { get; } = new DownloadAPIInfo
        {
            VersionManifest = "https://download.mcbbs.net/mc/game/version_manifest_v2.json",
            Launcher = "download.mcbbs.net",
            LauncherMeta = "download.mcbbs.net",
            Asset = "download.mcbbs.net/assets",
            Libraries = "download.mcbbs.net/maven",
            Forge = "download.mcbbs.net/maven",
            Liteloader = "https://download.mcbbs.net/maven/com/mumfrey/liteloader/versions.json",
            AuthLib = "download.mcbbs.net/mirrors/authlib-injector",
            FabricMaven = "download.mcbbs.net/maven",
            FabricMeta = "download.mcbbs.net/fabric-meta"
        };

        public static DownloadAPIInfo Current { get; set; } = Bmclapi;

        
    }

    public static class DownloadConfigs
    {
        public static int ThreadCount { get; set; } //全局变量 最大线程数

        public static int ErrorTryCount { get; set; } //全局变量 下载错误重试次数
    }
}
