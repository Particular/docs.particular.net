using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.StartupShutdown";
        #region Program

        Configure.Features.Enable<MyFeature>();
        Logger.WriteLine("Starting configuration");
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.StartupShutdown");
        configure.DefaultBuilder();
        configure.UseInMemoryTimeoutPersister();

        Logger.WriteLine("Calling Configure.UnicastBus");
        var unicastBus = configure.UnicastBus();
        Logger.WriteLine("Calling ConfigUnicastBus.CreateBus");
        using (var startableBus = unicastBus.CreateBus())
        {
            Logger.WriteLine("Calling IStartableBus.Start");
            var bus = startableBus
                .Start(() =>
                {
                    Logger.WriteLine("Calling ForInstallationOn.Install");
                    configure.ForInstallationOn<Windows>().Install();
                });

            //simulate some activity
            Thread.Sleep(500);

            Logger.WriteLine("Bus is processing messages");
            Logger.WriteLine("Calling IStartableBus.Shutdown");
            startableBus.Shutdown();
            Logger.WriteLine("Calling IStartableBus.Dispose");
        }
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}