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
            DoNotWrapOutgoingMessages = true
        };
        endpointConfiguration.UseTransport(transport);

        #region RegisterBehaviorInPipeline
        endpointConfiguration.Pipeline.Register(new AccessToAmazonSqsNativeMessageBehavior(), "Demonstrates how to access the native message from a pipeline behavior");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}