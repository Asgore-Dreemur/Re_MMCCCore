using MMCCCore.DataClasses.MC;
using System.ComponentModel;
using System.Net;

namespace MMCCCore.Functions.MC
{
    public static class MCAssetWrapper
    {
        static private string DownloadAPIInfo_URL = APIManager.Current.Asset + "/";
        /// <remarks>
        /// ������Դ�ļ�
        /// </remarks>
        /// <param name="gameRootPath">String .minecraft�ļ���·��</param>
        /// <param name="info">MCVersionJsonInfo �汾Json��Ϣ</param>
        /// <returns>InstallResult,���ؽ��̷���</returns>
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