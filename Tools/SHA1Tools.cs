using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MMCCCore.Tools
{
    public static class SHA1Tools
    {
        public static string GetSHA1(string path)
        {
            if (!File.Exists(path)) return string.Empty;
            FileStream stream = new FileStream(path, FileMode.Open);
            SHA1 sha1 = SHA1.Create();
            byte[] sha1byte = sha1.ComputeHash(stream);
            stream.Close();
            StringBuilder sb = new StringBuilder();
            foreach(var item in sha1byte)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }

        public static bool CompareSHA1(string path, string sha1value)
        {
            if (!File.Exists(path)) return false;
            FileStream stream = new FileStream(path, FileMode.Open);
            SHA1 sha1 = SHA1.Create();
            byte[] sha1byte = sha1.ComputeHash(stream);
            stream.Close();
            StringBuilder sb = new StringBuilder();
            foreach (var item in sha1byte)
            {
                sb.Append(item.ToString("x2"));
            }
            if (sb.ToString().ToLower() == sha1value.ToLower()) return true;
            return false;
        }
    }
}
