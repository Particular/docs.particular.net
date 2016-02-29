using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.ErrorHandling.WithoutSLR";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        #region DisableSLR
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ErrorHandling.WithoutSLR");
        busConfiguration.DisableFeature<SecondLevelRetries>();
        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message that will throw an exception.");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                MyMessage m = new MyMessage
                {
                    Id = Guid.NewGuid()
                };
                bus.SendLocal(m);
            }
        }
    }
}