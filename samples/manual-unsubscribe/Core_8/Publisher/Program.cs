﻿using NServiceBus;
using NServiceBus.Features;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Publisher";

        #region publisher-config

        var endpointConfiguration = new EndpointConfiguration("Samples.ManualUnsubscribe.Publisher");
        endpointConfiguration.UseTransport(new MsmqTransport());

        var persistence = endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        persistence.SubscriptionQueue("Samples.ManualUnsubscribe.Publisher.Subscriptions");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        endpointConfiguration.EnableInstallers();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press 'enter' to publish the SomethingHappened event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.Enter)
            {
                var somethingHappened = new SomethingHappened();
                await endpointInstance.Publish(somethingHappened);
                Console.WriteLine("Published SomethingHappened Event.");
            }
            else
            {
                break;
            }
        }

        await endpointInstance.Stop();
    }
}