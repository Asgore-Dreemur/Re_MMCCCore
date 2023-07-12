using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.MC
{
    /// <summary>
    /// 用于存储MC支持库信息的数据类
    /// </summary>
    public class MCLibraryInfo
    {
        public string Name { get; set; } //支持库命名空间
        
        public MCVersionJsonLibraryInfo RawJson { get; set; } //支持库原json

        public string Path { get; set; } //支持库应该被储存到的相对路径

        public int Size { get; set; } //支持库大小(单位B)

        public string Url { get; set; } //下载Url

        public string Sha1 { get; set; } //校验sha1

        public bool isNative { get; set; } //是否是native

        public bool isEnable { get; set; } //是否需要对当前系统启用
    }
}
