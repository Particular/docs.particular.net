using System;

namespace PlatformLauncher.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "PlatformLauncher";
            Particular.PlatformLauncher.Launch();
        }
    }
}
