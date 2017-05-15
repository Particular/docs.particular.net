using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

class Program
{
    #region ClientInit
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        // This makes it easier to tell console windows apart
        Console.Title = "Samples.StepByStep.Client";

        // The endpoint name will be used to determine queue names and serves
        // as the address, or identity, of the endpoint
        var endpointConfiguration = new EndpointConfiguration(
            endpointName: "Samples.StepByStep.Client");

        endpointConfiguration.SendFailedMessagesTo("error");

        // Use JSON to serialize and deserialize messages (which are just
        // plain classes) to and from message queues
        endpointConfiguration.UseSerialization<JsonSerializer>();

        // Ask NServiceBus to automatically create message queues
        endpointConfiguration.EnableInstallers();

        // Store messages on disk for this example, rather than in
        // a real queue.
        endpointConfiguration.UseTransport<LearningTransport>();

        // Store information on disk for this example, rather than in
        // a database. In this sample, only subscription information is stored
        endpointConfiguration.UsePersistence<LearningPersistence>();

        // Initialize the endpoint with the finished configuration
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await SendOrder(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
    #endregion


    #region SendOrder
    static async Task SendOrder(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            var id = Guid.NewGuid();

            var placeOrder = new PlaceOrder
            {
                Product = "New shoes",
                Id = id
            };
            await endpointInstance.Send("Samples.StepByStep.Server", placeOrder);
            Console.WriteLine($"Sent a PlaceOrder message with id: {id:N}");
        }
    }
    #endregion
}
