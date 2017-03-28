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
        var endpointName = "Samples.NHibernateCustomSagaFinder";
        Console.Title = endpointName;
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();
        routing.RegisterPublisher(typeof(Program).Assembly, endpointName);

        #region NHibernateSetup

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=NHCustomSagaFinder;Integrated Security=True");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await endpointInstance.SendLocal(startOrder)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}