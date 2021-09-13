using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PipelineStream.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.PipelineStream.Receiver");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        endpointConfiguration.Pipeline.Register<StreamSendBehavior.Registration>();
        endpointConfiguration.Pipeline.Register(typeof(StreamReceiveBehavior), "Copies the shared data back to the logical messages");
        endpointConfiguration.SetStreamStorageLocation(@"..\..\..\..\storage");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}