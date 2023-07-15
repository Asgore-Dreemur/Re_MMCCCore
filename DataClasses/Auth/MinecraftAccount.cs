using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.DataClasses.Auth
{
    public class MinecraftAccount
    {
        public string Name { get; set; }

        public string UUID { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public Exception ErrorException { get; set; }
    }
}
