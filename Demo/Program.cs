using System;
using MMCCCore.Functions.MC;

namespace Program
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var versions = MCVersionWrapper.GetAllMCVersions();
            Console.WriteLine(versions);
        }
    }
}