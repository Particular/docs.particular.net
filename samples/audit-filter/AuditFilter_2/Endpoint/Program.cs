using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AuditFilter;

class Program
{
    public static string AuditPath = Path.GetFullPath("../.learningtransport/audit");

    static async Task Main()
    {
        Console.Title = "Samples.AuditFilter";
        var endpointConfiguration = new EndpointConfiguration("Samples.AuditFilter");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region Enable

        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.FilterAuditQueue(
            defaultFilter: FilterResult.IncludeInAudit);

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine($"Audit Path:\r\n{AuditPath}");
        Console.WriteLine("Press E to send a message that will be excluded");
        Console.WriteLine("Press I to send a message that will be included");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            Console.WriteLine();
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.I)
            {
                var message = new MessageToIncludeAudit();
                await endpoint.SendLocal(message)
                    .ConfigureAwait(false);
                continue;
            }
            if (key.Key == ConsoleKey.E)
            {
                var message = new MessageToExcludeFromAudit();
                await endpoint.SendLocal(message)
                    .ConfigureAwait(false);
                continue;
            }
            break;
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}