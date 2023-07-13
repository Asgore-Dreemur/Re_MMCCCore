using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Tools
{
    public static class OtherTools
    {
        /// <summary>
        /// 获取MC式的系统类型名称
        /// </summary>
        /// <returns>系统类型名称,只返回windows nt, unix和macos对应的系统类型,其余为null</returns>
        public static string GetSystemPlatformLikesMC()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                    return null;
                case PlatformID.Win32Windows:
                    return null;
                case PlatformID.Win32NT:
                    return "windows";
                case PlatformID.WinCE:
                    return null;
                case PlatformID.Unix:
                    return "linux";
                case PlatformID.Xbox:
                    return null;
                case PlatformID.MacOSX:
                    return "osx";
                case PlatformID.Other:
                    return null;
                default:
                    return null;
            }
        }

        public static string FormatPath(string path) => path.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);

        public static void CreateDirs(string path)
        {
            string cpath = FormatPath(path).TrimEnd(Path.DirectorySeparatorChar);
            string[] sarray = cpath.Split(Path.DirectorySeparatorChar);
            string currpath = sarray[0];
            for(int i=1;i<sarray.Length;i++)
            {
                currpath = Path.Combine(currpath, sarray[i]);
                if (!Directory.Exists(currpath)) Directory.CreateDirectory(currpath);
            }
        }
    }
}
