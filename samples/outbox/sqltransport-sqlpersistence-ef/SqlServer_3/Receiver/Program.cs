using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SQLOutboxEF.Receiver";

        var connectionString = @"Data Source=.\SqlExpress;Database=nservicebus;Integrated Security=True";
        using (var receiverDataContext = new ReceiverDataContext(connectionString))
        {
            receiverDataContext.Database.Initialize(true);
        }

        var endpointConfiguration = new EndpointConfiguration("Samples.SQLOutboxEF.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        transport.DefaultSchema("receiver");
        transport.UseSchemaForEndpoint("Samples.SQLOutboxEF.Sender", "sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted).Assembly, "Samples.SQLOutboxEF.Sender");
        routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "Samples.SQLOutboxEF.Sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });
        persistence.Schema("receiver");
        persistence.TablePrefix("");

        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.DisableCache();

        endpointConfiguration.EnableOutbox();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}