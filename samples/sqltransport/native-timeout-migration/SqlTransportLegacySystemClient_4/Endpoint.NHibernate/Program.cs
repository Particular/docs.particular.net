using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;

class Program
{
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesNativeTimeoutMigration;Integrated Security=True;Max Pool Size=100;Encrypt=false
    const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesNativeTimeoutMigration;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

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
        await SqlHelper.EnsureDatabaseExists(ConnectionString);

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
