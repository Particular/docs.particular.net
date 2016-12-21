﻿using System;
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
        Console.Title = "Samples.CustomRouting.Sales.1";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Sales");
        endpointConfiguration.OverrideLocalAddress("Samples.CustomRouting.Sales-1");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(AutomaticRoutingConst.ConnectionString);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region EnableAutomaticRouting

        endpointConfiguration.EnableAutomaticRouting(AutomaticRoutingConst.ConnectionString);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}