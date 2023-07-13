using MMCCCore.DataClasses.MC;
using System.ComponentModel;
using System.Net;

namespace MMCCCore.Functions.MC
{
    public static class MCAssetWrapper
    {
        static private string DownloadAPIInfo_URL = APIManager.Current.Asset + "/";
        /// <remarks>
        /// 下载资源文件
        /// </remarks>
        /// <param name="gameRootPath">String .minecraft文件夹路径</param>
        /// <param name="info">MCVersionJsonInfo 版本Json信息</param>
        /// <returns>InstallResult,下载进程返回</returns>
        public static InstallResult DownloadAssets(MCVersionJsonInfo info,string gameRootPath)
        {
            string filehash = info.AssetIndex.Sha1;
            int filesize = info.AssetIndex.Size;
            string url = DownloadAPIInfo_URL + filehash.Substring(0, 1) + "/" + filehash + "/";
            
            return new InstallResult
            {
                isSuccess = true,
                Message = "ok"
            };
        }
    }
}