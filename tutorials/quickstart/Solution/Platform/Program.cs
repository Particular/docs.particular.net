using System;
using System.Threading.Tasks;

namespace Platform
{
    class Program
    {
        #region PlatformMain

        static async Task Main()
        {
            Console.Title = "Particular Service Platform Launcher";
            await Particular.PlatformLauncher.Launch();
        }

        #endregion
    }
}