using MMCCCore.DataClasses.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Tools
{
    public class MultiFileDownloader
    {
        public Stack<DownloadTaskInfo> DownloadStack { get; set; }

        public int TheadCount { get; set; }

        private bool IsStop { get; set; } = false;

        private DownloadResult LastDownloadResult { get; set; } = null;

        private object locker = new object(), locker2 = new object(), locker3 = new object();

        private List<Thread> ThreadPool { get; set; } = new List<Thread>();

        public event EventHandler<(int, int)> OnProgressChanged;

        private int Downloaded { get; set; } = 0;

        private int AllFileCount { get; set; } = 0;

        public MultiFileDownloader() { }

        /// <summary>
        /// 初始化下载器
        /// </summary>
        /// <param name="stack">下载队列</param>
        /// <param name="count">最大线程数</param>
        public MultiFileDownloader(Stack<DownloadTaskInfo> stack, int tcount)
        {
            this.DownloadStack = stack;
            this.TheadCount = tcount;
            this.AllFileCount = stack.Count;
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void StartDownload()
        {
            ThreadPool.Clear();
            this.AllFileCount = DownloadStack.Count;
            this.Downloaded = 0;
            LastDownloadResult = null;
            for (int i = 0; i < TheadCount; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(DownloadThread));
                thread.Start(DownloadStack);
                ThreadPool.Add(thread);
            }
        }

        /// <summary>
        /// 停止下载,执行后应执行WaitForDownloadComplete等待
        /// </summary>
        public void StopDownload() => IsStop = true;

        public void DownloadThread(object arg)
        {
            var stack = (Stack<DownloadTaskInfo>)arg;
            while (true)
            {
                DownloadTaskInfo info;
                lock (locker)
                {
                    if (stack.Count <= 0 || IsStop) return;
                    info = stack.Pop();
                }
                var result = FileDownloader.DownloadFile(info);
                if (!result.IsSuccess)
                {
                    LastDownloadResult = result;
                    IsStop = true;
                    return;
                }
                lock (locker2) Downloaded++;
                RunOnProgressChanged();
            }
        }

        private async Task RunOnProgressChanged()
        {
            lock (locker3) OnProgressChanged?.Invoke(this, (Downloaded, AllFileCount));
        }


        /// <summary>
        /// 等待下载完成
        /// </summary>
        /// <returns>下载结果，若下载中出现错误，则返回最后一个错误的下载结果</returns>
        public DownloadResult WaitForDownloadComplete()
        {
            foreach (var item in ThreadPool)
            {
                item.Join();
            }
            if (LastDownloadResult == null) return new DownloadResult { IsSuccess = true };
            return LastDownloadResult;
        }
    }
}
