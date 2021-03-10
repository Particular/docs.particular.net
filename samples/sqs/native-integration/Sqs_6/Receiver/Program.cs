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
        endpointConfiguration.UseTransport(new SqsTransport());

        #region RegisterBehaviorInPipeline
        endpointConfiguration.Pipeline.Register(new AccessToAmazonSqsNativeMessageBehavior(), "access-to-native-msg");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}