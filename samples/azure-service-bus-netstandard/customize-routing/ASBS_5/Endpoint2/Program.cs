using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Endpoint2";

        var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.SendReply.Endpoint2");
        endpointConfiguration.EnableInstallers();


        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
        endpointConfiguration.UseTransport(transport);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.Pipeline.Register(new PublishAllMessagesBehavior(), "Overrides message delivery mode");
        endpointConfiguration.Pipeline.Register(new PublishSendsBehavior(), "Overrides sends delivery mode");
        endpointConfiguration.Pipeline.Register(new PublishRepliesBehavior(), "Overrides replies delivery mode");

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        await endpointInstance.Subscribe<Message1>();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}