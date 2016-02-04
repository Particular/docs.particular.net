using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region sqlsubscriber-config

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EndpointName("SqlRelay");
        busConfiguration.EnableInstallers();
        busConfiguration.DisableFeature<AutoSubscribe>();
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");
        busConfiguration.UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");

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
        Console.WriteLine("\r\nSqlRelay is running - This endpoint will relay all events received to subscribers. Press any key to stop program\r\n");
        Console.ReadKey();
    }
}