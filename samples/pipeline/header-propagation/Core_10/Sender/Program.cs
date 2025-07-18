using System;
using System.Threading.Tasks;
using NServiceBus;

public static class Program
{
    public static async Task Main()
    {
        Console.Title = "Sender";

        var endpointConfiguration = new EndpointConfiguration("Sender");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        transport.Routing().RouteToEndpoint(typeof(ProcessOrder), "Receiver");

        var endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press [ESC] to quit. Any other key to send a message.");

        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            #region send-message
            var customerId = Guid.NewGuid().ToString();
            var sendOptions = new SendOptions();
            sendOptions.SetHeader("CustomerId", customerId);

            await endpoint.Send(new ProcessOrder(), sendOptions);
            Console.WriteLine($"Message sent, customerId = {customerId}.");
            #endregion
        }

        await endpoint.Stop();
    }
}
