using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.ComplexSagaFindingLogic";
        var endpointConfiguration = new EndpointConfiguration("Samples.ComplexSagaFindingLogic");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await endpointInstance.SendLocal(new StartOrder
                          {
                              OrderId = "123"
                          })
                          .ConfigureAwait(false);
            await endpointInstance.SendLocal(new StartOrder
                          {
                              OrderId = "456"
                          })
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
