using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "NServiceBusEndpoint";
        var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: "Particular.ServiceControl",
            frequency: TimeSpan.FromSeconds(10),
            timeToLive: TimeSpan.FromSeconds(30));

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

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
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
            await endpointInstance.Send("NServiceBusEndpoint", simpleMessage);
            Console.WriteLine($"Sent a new message with Id = {guid}.");

            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
        }
        await endpointInstance.Stop();
    }
}