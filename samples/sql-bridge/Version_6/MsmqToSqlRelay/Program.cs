namespace MsmqToSqlRelay
{
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

            #region bridge-config

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.SendFailedMessagesTo("error");
            busConfiguration.EndpointName("MsmqToSqlRelay");
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<NHibernatePersistence>()
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
            Console.WriteLine("\r\nMssqToSql Bridge is now running -- Press any key to stop program\r\n");
            Console.ReadKey();
        }

    }
}