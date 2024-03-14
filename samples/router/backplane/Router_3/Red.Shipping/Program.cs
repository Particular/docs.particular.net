﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Red.Shipping";
        var endpointConfiguration = new EndpointConfiguration("Red.Shipping");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionStrings.Red);

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}