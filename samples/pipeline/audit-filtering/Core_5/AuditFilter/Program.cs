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
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var auditThisMessage = new AuditThisMessage
            {
                Content = "See you in the audit queue!"
            };
            bus.SendLocal(auditThisMessage);

            var doNotAuditThisMessage = new DoNotAuditThisMessage
            {
                Content = "Don't look for me!"
            };
            bus.SendLocal(doNotAuditThisMessage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}