﻿using System;
using System.Threading.Tasks;
using NServiceBus;
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
        transport.UseNativeDelayedDelivery().DisableTimeoutManagerCompatibility();

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("The endpoint has started. Run the script to migrate the timeouts.");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);

    }
}