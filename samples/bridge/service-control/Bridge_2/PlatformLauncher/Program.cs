namespace PlatformLauncher
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "PlatformLauncher";
            await Particular.PlatformLauncher.Launch();
        }
    }
}