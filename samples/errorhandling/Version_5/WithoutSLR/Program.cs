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

        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.ErrorHandling.WithoutSLR");
        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.DisableFeature<SecondLevelRetries>();
        configuration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(configuration))
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