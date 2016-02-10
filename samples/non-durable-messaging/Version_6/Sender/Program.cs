using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region non-transactional

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.UseTransport<MsmqTransport>()
            .Transactions(TransportTransactionMode.None);

        #endregion

        endpointConfiguration.EndpointName("Samples.MessageDurability.Sender");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            await endpoint.Send("Samples.MessageDurability.Receiver", new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}