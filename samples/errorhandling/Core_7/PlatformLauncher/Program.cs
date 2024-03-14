using System;
using System.Threading.Tasks;

namespace PlatformLauncher.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "PlatformLauncher";
            await Particular.PlatformLauncher.Launch();
        }
    }
}