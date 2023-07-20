using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses
{
    public class LangData
    {
        public string Microsoft_XBLAuthing { get; set; }

        public string Microsoft_XSTSAuthing { get; set; }

        public string Microsoft_XboxLoggingIn { get; set; }

        public string Microsoft_GettingPlayerProfile { get; set; }

        public string Microsoft_HasNoMinecraft { get; set; }

        public string Microsoft_XboxLoginError { get; set; }

        public string Microsoft_ProfileGettingError { get; set; }

        public string Microsoft_CheckingHasMinecraft { get; set; }

        public string Microsoft_DeviceFlowFailed { get; set; }

        public string Optifine_Downloading { get; set; }

        public string Optifine_WritingFile { get; set; }

        public string Optifine_RunningProcess { get; set; }

        public string Optifine_ProcessError { get; set; }

        public string CannotFindRawJar { get; set; }

        public string ExistsVersionError { get; set; }
    }

    public static class LocalLang
    {
        public static LangData ZH_CN { get; } = new LangData
        {
            Microsoft_XBLAuthing = "XBL验证中",
            Microsoft_XSTSAuthing = "XSTS验证中",
            Microsoft_XboxLoggingIn = "Xbox登录中",
            Microsoft_GettingPlayerProfile = "账号信息获取中",
            Microsoft_HasNoMinecraft = "你没有购买正版Minecraft",
            Microsoft_XboxLoginError = "Xbox登录失败:未知错误",
            Microsoft_CheckingHasMinecraft = "正在检查Minecraft拥有",
            Microsoft_ProfileGettingError = "未知错误,无法获取账号信息",
            Microsoft_DeviceFlowFailed = "设备代码流验证失败,原因可能是超时(限时15分),你主动拒绝了操作或未知错误",
            Optifine_Downloading = "下载Optifine",
            Optifine_WritingFile = "写入文件中",
            Optifine_RunningProcess = "安装Optifine",
            Optifine_ProcessError = "安装时出现错误:{0}",
            CannotFindRawJar = "找不到该版本的原版核心jar,请安装原版版本,若已安装,请先启动一次后重试",
            ExistsVersionError = "该版本已存在"
        };

        public static LangData EN_US { get; } = new LangData
        {
            Microsoft_XBLAuthing = "XBL validating",
            Microsoft_XSTSAuthing = "XSTS vaildating",
            Microsoft_XboxLoggingIn = "Logging in Xbox",
            Microsoft_GettingPlayerProfile = "Getting player profile",
            Microsoft_HasNoMinecraft = "You don't have Minecraft",
            Microsoft_XboxLoginError = "Cannot login xbox,unkown error",
            Microsoft_CheckingHasMinecraft = "Checking Minecraft on this Account",
            Microsoft_ProfileGettingError = "Unkown error,cannot get account info",
            Microsoft_DeviceFlowFailed = "Device flow vaildation failed.May timeout(in 15 minutes),refused the operation or unkown error",
            Optifine_Downloading = "Downloading Optifine",
            Optifine_WritingFile = "Writing files",
            Optifine_RunningProcess = "Installing Optifine",
            Optifine_ProcessError = "Error while install optifine:{0}",
            CannotFindRawJar = "Cannot find the jar of the raw version of the version,install it,if installed,launch it and try again",
            ExistsVersionError = "The version is exists"
        };

        public static LangData Current { get; set; } = EN_US; //当前使用的语言

        public static string CurrentLang { get; set; } = "en-US"; //xx-XX式语言代码
    }
}
