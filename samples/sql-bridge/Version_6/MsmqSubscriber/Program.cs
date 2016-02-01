using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {

        #region msmqsubscriber-config

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EndpointName("MsmqSubscriber");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForMsmqTransport;Integrated Security=True");

        #endregion


        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Start(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }


    static void Start(IBusSession busSession)
    {
        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }

}