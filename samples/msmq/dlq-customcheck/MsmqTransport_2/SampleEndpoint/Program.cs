using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        MsmqUtils.SetUpDummyQueue();
        var endpointConfiguration = new EndpointConfiguration("SampleEndpoint");
        endpointConfiguration.UseTransport(new MsmqTransport());
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        // disable delayed retries because NonDurablePersistence doesn't support timeouts:
        endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region configure-custom-checks

        endpointConfiguration.ReportCustomChecksTo("Particular.ServiceControl");

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Endpoint Started");
        Console.WriteLine("Press [d] to send a message to the Dead Letter Queue");
        Console.WriteLine("Press any other key to exit");

        while (Console.ReadKey(true).Key == ConsoleKey.D)
        {
            MsmqUtils.SendMessageToDeadLetterQueue(DateTime.UtcNow.ToShortTimeString());
            Console.WriteLine("Sent message to Dead Letter Queue");
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}