using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ConsumerDrivenContracts.Consumer2";
        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Consumer2");
        var transport = endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();

        await endpointInstance.Stop();
    }
}