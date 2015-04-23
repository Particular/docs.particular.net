using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ErrorHandling.WithoutSLR");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.DisableFeature<SecondLevelRetries>();
        busConfiguration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("Press any key to send a message that will throw an exception.");
            Console.WriteLine("To exit, press Ctrl + C");

            while (true)
            {
                Console.ReadLine();
                MyMessage m = new MyMessage
                              {
                                  Id = Guid.NewGuid()
                              };
                bus.SendLocal(m);
            }
        }
    }
}