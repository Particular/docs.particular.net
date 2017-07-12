using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;

class Program
{
    const string ConnectionString = @"Data Source=.\SqlExpress;Database=nservicebus;Integrated Security=True";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.NativeTimeoutMigration";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.NativeTimeoutMigration");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionString);
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SubscriptionSettings().DisableCache();
        persistence.ConnectionBuilder(() => new SqlConnection(ConnectionString));
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        options.DelayDeliveryWith(TimeSpan.FromSeconds(10));
        await endpointInstance.Send(new MyMessage(), options).ConfigureAwait(false);

        //Ensure timeout message is processed and stored in the database
        await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);

        Console.WriteLine("The timeout has been requested. Press any key to exit");
        Console.ReadKey();
    }
}