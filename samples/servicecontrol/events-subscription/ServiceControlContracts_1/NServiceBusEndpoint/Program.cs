using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "NServiceBusEndpoint";
        var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region DisableRetries

        var recoverability = endpointConfiguration.Recoverability();

        recoverability.Delayed(
            customizations: retriesSettings =>
            {
                retriesSettings.NumberOfRetries(0);
            });
        recoverability.Immediate(
            customizations: retriesSettings =>
            {
                retriesSettings.NumberOfRetries(0);
            });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
        while (true)
        {
            var key = Console.ReadKey();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var guid = Guid.NewGuid();

            var simpleMessage = new SimpleMessage
            {
                Id = guid
            };
            await endpointInstance.Send("NServiceBusEndpoint", simpleMessage)
                .ConfigureAwait(false);
            Console.WriteLine($"Sent a new message with Id = {guid}.");

            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}