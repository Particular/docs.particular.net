namespace PlatformLauncher
{
    using System;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "PlatformLauncher";
            await Particular.PlatformLauncher.Launch(servicePulseDefaultRoute: "/custom-checks")
                .ConfigureAwait(false);
        }
    }
}
