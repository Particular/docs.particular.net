﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlPersistence.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlPersistence.Client");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press 'enter' to send a StartOrder messages");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };
            await endpointInstance.Send("Samples.SqlPersistence.Server", startOrder)
                .ConfigureAwait(false);
            Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}