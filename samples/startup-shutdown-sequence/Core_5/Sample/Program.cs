using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.StartupShutdown";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        #region Program
        Logger.WriteLine("Starting configuration");
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StartupShutdown");
        busConfiguration.EnableInstallers();
        busConfiguration.EnableFeature<MyFeature>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        Logger.WriteLine("Calling Bus.Create");
        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            Logger.WriteLine("Calling IStartableBus.Create");
            bus.Start();

            //simulate some activity
            Thread.Sleep(500);

            Logger.WriteLine("Bus is processing messages");
            Logger.WriteLine("Calling IStartableBus.Dispose");
        }
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}