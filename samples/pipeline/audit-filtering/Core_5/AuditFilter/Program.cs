using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.AuditFilter";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.AuditFilter");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal<AuditThisMessage>(m =>
            {
                m.Content = "See you in the audit queue!";
            });

            bus.SendLocal<DoNotAuditThisMessage>(m =>
            {
                m.Content = "Don't look for me!";
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
