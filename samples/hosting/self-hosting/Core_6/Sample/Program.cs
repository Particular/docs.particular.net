using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.SelfHosting";
        #region self-hosting

        var endpointConfiguration = new EndpointConfiguration("Samples.SelfHosting");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            var myMessage = new MyMessage();
            await endpointInstance.SendLocal(myMessage)
                .ConfigureAwait(false);
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        #endregion
    }
}