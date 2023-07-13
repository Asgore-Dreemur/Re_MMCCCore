using MMCCCore.DataClasses.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Tools
{
    public class FileDownloader
    {
        //第一个long:已下载字节数  第二个long:文件总共字节数
        public event EventHandler<(long, long)> OnProgressChanged;

        /// <summary>
        /// 下载单个文件,异步函数
        /// </summary>
        /// <param name="info">下载文件任务信息</param>
        /// <returns>下载结果</returns>
        public async Task<DownloadResult> DownloadFileTaskAsync(DownloadTaskInfo info)
        {
            int ErrorCount = 0;
            Exception exception = null;
            while (ErrorCount < info.TryCount)
            {
                try
                {
                    if (!string.IsNullOrEmpty(info.Sha1) && SHA1Tools.CompareSHA1(info.SavePath, info.Sha1))
                        return new DownloadResult { IsSuccess = true, Task = info };
                    FileStream fstream = new FileStream(info.SavePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var result = HttpTools.HttpGetTaskAsync(info.Url).GetAwaiter().GetResult();
                    long? length = result.Content.Headers.ContentLength;
                    var stream = result.Content.ReadAsStream();
                    byte[] data = new byte[1024];
                    int size = stream.Read(data, 0, data.Length);
                    int wrote = 0;
                    while (size > 0)
                    {
                        fstream.Write(data, 0, size);
                        wrote += size;//记录字节数
                        OnProgressChanged?.Invoke(this, (wrote, length.HasValue ? length.Value : -1));
                        size = stream.Read(data, 0, data.Length);
                    }
                    stream.Close();
                    fstream.Close();
                    if(!string.IsNullOrEmpty(info.Sha1))
                    {
                        string sha1 = SHA1Tools.GetSHA1(info.SavePath);
                        if (info.Sha1.ToLower() != sha1.ToLower())
                        {
                            return new DownloadResult
                            {
                                Task = info,
                                ErrorException = new Exception($"SHA1不匹配:{info.SavePath}(应为{info.Sha1})"),
                                IsSuccess = false
                            };
                        }
                    }
                    return new DownloadResult { IsSuccess = true, Task = info };
                }
                catch (Exception e)
                {
                    exception = e;
                    ErrorCount++;
                }
            }
            return new DownloadResult
            {
                Task = info,
                ErrorException = exception,
                IsSuccess = false
            };
        }

        /// <summary>
        /// 静态函数下载文件，不显示进度
        /// </summary>
        /// <param name="info">下载任务信息</param>
        /// <returns>下载结果</returns>
        public static DownloadResult DownloadFile(DownloadTaskInfo info)
        {
            int ErrorCount = 0; //错误次数
            Exception exception = null; //暂时存储Error
            while (ErrorCount < info.TryCount) //控制错误次数
            {
                try
                {
                    if (!string.IsNullOrEmpty(info.Sha1) && SHA1Tools.CompareSHA1(info.SavePath, info.Sha1))
                        return new DownloadResult { IsSuccess = true, Task = info };
                    FileStream fstream = new FileStream(info.SavePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var result = HttpTools.HttpGetTaskAsync(info.Url).GetAwaiter().GetResult();
                    long? length = result.Content.Headers.ContentLength;
                    var stream = result.Content.ReadAsStream();
                    byte[] data = new byte[1024];
                    int size = stream.Read(data, 0, data.Length);
                    while (size > 0)
                    {
                        fstream.Write(data, 0, size);
                        size = stream.Read(data, 0, data.Length);
                    }
                    stream.Close();
                    fstream.Close();
                    if (!string.IsNullOrEmpty(info.Sha1)) //如果sha1不为空，即启用sha1验证
                    {
                        string sha1 = SHA1Tools.GetSHA1(info.SavePath);
                        if (info.Sha1.ToLower() != sha1.ToLower()) //排除大小写问题
                        {
                            return new DownloadResult
                            {
                                Task = info,
                                ErrorException = new Exception($"SHA1不匹配:{info.SavePath}(应为{info.Sha1})"),
                                IsSuccess = false
                            };
                        }
                    }
                    return new DownloadResult { IsSuccess = true, Task = info };
                }
                catch (Exception e)
                {
                    exception = e;
                    ErrorCount++; //增加错误次数
                }
            }
            return new DownloadResult
            {
                Task = info,
                ErrorException = exception,
                IsSuccess = false
            };
        }
    }
}
