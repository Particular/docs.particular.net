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
        EndpointConfiguration busConfiguration = new EndpointConfiguration("Samples.ComplexSagaFindingLogic");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            await endpoint.SendLocal(new StartOrder
                          {
                              OrderId = "123"
                          });
            await endpoint.SendLocal(new StartOrder
                          {
                              OrderId = "456"
                          });
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}
