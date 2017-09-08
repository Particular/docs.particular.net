using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;

class Program
{
    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SQLOutboxEF.Receiver";

        var connection = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlOutbox;Integrated Security=True;Max Pool Size=100";

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlOutbox.Receiver");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region ReceiverConfiguration

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.DefaultSchema("receiver");
        transport.UseSchemaForEndpoint("Samples.SqlOutbox.Sender", "sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(OrderAccepted).Assembly, "Samples.SqlOutbox.Sender");
        routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "Samples.SqlOutbox.Sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("receiver");
        persistence.TablePrefix("");

        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.DisableCache();

        endpointConfiguration.EnableOutbox();

        #endregion
        SqlHelper.CreateSchema(connection, "receiver");

        SqlHelper.ExecuteSql(connection, File.ReadAllText("Startup.sql"));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}