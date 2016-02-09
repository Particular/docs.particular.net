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
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageBodyEncryption.Endpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        #region RegisterMessageEncryptor
        busConfiguration.RegisterMessageEncryptor();
        #endregion
        busConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            CompleteOrder completeOrder = new CompleteOrder
                                          {
                                              CreditCard = "123-456-789"
                                          };
            await endpoint.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder);
            Console.WriteLine("Message sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}