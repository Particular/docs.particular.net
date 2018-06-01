using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Sqs.Simple";
        #region ConfigureEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Sqs.Simple");
        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.S3("andreas-sqs", "large");

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        #region sendonly
        //TODO: uncomment to view a message in transit
        //endpointConfiguration.SendOnly();
        #endregion
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region sends
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);

        var myLargeMessage = new MyMessage
        {
            Data = new byte[257 * 1024]
        };
        await endpointInstance.SendLocal(myLargeMessage)
            .ConfigureAwait(false);

        #endregion
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}