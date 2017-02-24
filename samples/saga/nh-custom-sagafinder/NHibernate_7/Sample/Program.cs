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
        Console.Title = "Samples.NHibernateCustomSagaFinder";
        var endpointConfiguration = new EndpointConfiguration("Samples.NHibernateCustomSagaFinder");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region NHibernateSetup

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=NHCustomSagaFinder;Integrated Security=True");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            var startOrder = new StartOrder
            {
                OrderId = "123"
            };
            await endpointInstance.SendLocal(startOrder)
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