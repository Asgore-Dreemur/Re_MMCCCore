using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MMCCCore.DataClasses.Auth.Microsoft
{
    public class MicrosoftAccountInfo : MinecraftAccount
    {
        public PlayerProfileInfo Profile { get; set; }
    }
}
