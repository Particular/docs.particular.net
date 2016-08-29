using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "NServiceBusEndpoint";
        var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region DisableRetries

        var recoverability = endpointConfiguration.Recoverability();

        recoverability.Delayed(c => c.NumberOfRetries(0));
        recoverability.Immediate(c => c.NumberOfRetries(0));

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                var guid = Guid.NewGuid();

                await endpointInstance.Send("NServiceBusEndpoint", new SimpleMessage { Id = guid });
                Console.WriteLine($"Sent a new message with Id = {guid}.");

                Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}