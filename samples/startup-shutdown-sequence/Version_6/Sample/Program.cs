using System;
using System.Threading;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
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

            //simulate some bus activity
            Thread.Sleep(500);

            Logger.WriteLine("Bus is processing messages");
            Logger.WriteLine("Calling IStartableBus.Dispose");
        }
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    // TODO: Remove when https://github.com/Particular/NServiceBus/issues/2913 is fixed
    class Provide : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig();
        }
    }
}