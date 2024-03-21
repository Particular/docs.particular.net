using System;
using System.Threading.Tasks;

namespace Platform
{
    class Program
    {
        #region PlatformMain

        static async Task Main()
        {
            Console.Title = "Platform Launcher";
            await Particular.PlatformLauncher.Launch();
        }

        #endregion
    }
}