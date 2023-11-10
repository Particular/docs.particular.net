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

        #region RegisterBehaviorInPipeline
        endpointConfiguration.Pipeline.Register(new AccessToAmazonSqsNativeMessageBehavior(), "Demonstrates how to access the native message from a pipeline behavior");
        endpointConfiguration.Pipeline.Register(new PopulateReplyToAddressBehavior(), "Behavior to enable replies back to the native endpoint");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}