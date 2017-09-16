using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    const string ConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesNativeTimeoutMigration;Integrated Security=True";

    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.NativeTimeoutMigration";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.NativeTimeoutMigration");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionString);
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(ConnectionString);
        endpointConfiguration.EnableInstallers();
        SqlHelper.EnsureDatabaseExists(ConnectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        options.DelayDeliveryWith(TimeSpan.FromSeconds(10));
        await endpointInstance.Send(new MyMessage(), options)
            .ConfigureAwait(false);

        //Ensure timeout message is processed and stored in the database
        await Task.Delay(TimeSpan.FromSeconds(5))
            .ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);

        Console.WriteLine("The timeout has been requested. Press any key to exit");
        Console.ReadKey();
    }
}