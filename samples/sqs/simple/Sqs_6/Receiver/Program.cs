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

        var transport = new SqsTransport
        {
            S3 = new S3Settings("bucketname", "my/key/prefix")
        };
        endpointConfiguration.UseTransport(transport);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}