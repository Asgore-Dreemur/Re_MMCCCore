using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Auth.Yggdrasil
{
    public class YggdrasilAccount : MinecraftAccount
    {
        public List<YProfileOverInfo> AvailableProfiles { get; set; } = new List<YProfileOverInfo>();
    }
}
