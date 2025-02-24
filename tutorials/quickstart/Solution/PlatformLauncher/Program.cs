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
            // The Particular Service Platform tools are disabled initially.
            // To enable them, replace the line below with:
            //   await Particular.PlatformLauncher.Launch();
            await Task.CompletedTask;
        }

        #endregion
    }
}