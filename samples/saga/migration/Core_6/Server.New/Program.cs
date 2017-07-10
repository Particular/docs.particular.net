//#define POST_MIGRATION

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SagaMigration.Server.New";
        var endpointConfiguration = new EndpointConfiguration("Samples.SagaMigration.Server");

#if !POST_MIGRATION
        endpointConfiguration.OverrideLocalAddress("Samples.SagaMigration.Server.New");
#endif

        endpointConfiguration.UseTransport<LearningTransport>();
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=nservicebus;Integrated Security=True;");
            });
        persistence.TablePrefix("New");

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit.");
        Console.ReadLine();

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}