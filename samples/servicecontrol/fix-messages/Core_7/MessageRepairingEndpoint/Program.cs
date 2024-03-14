using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "MessageRepairingEndpoint";
        var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.MessageRepairingEndpoint");
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press 'Enter' to finish.");
        Console.ReadLine();

        await endpointInstance.Stop();
    }
}