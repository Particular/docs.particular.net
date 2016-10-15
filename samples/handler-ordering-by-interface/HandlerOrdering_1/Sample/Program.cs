using System;
using System.Threading.Tasks;
using HandlerOrdering;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.HandlerOrdering";
        var endpointConfiguration = new EndpointConfiguration("Samples.HandlerOrdering");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        #region config
        endpointConfiguration.ApplyInterfaceHandlerOrdering();
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            var myMessage = new MyMessage();
            await endpointInstance.SendLocal(myMessage)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}