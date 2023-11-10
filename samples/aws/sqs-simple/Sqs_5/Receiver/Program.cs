using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Sqs.SimpleReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.SimpleReceiver");
        endpointConfiguration.EnableInstallers();
        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.S3("bucketname", "my/key/prefix");

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}