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
        Console.Title = "Samples.CustomDistributionStrategy.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomDistributionStrategy.Client");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region Logical-Routing

        var routing = endpointConfiguration.UnicastRouting();
        routing.RouteToEndpoint(typeof(DoSomething), "Samples.CustomDistributionStrategy.Server");
        //Distribute all messages using weighted algorithm
        routing.Mapping.SetMessageDistributionStrategy(new WeightedDistributionStrategy(), t => true);

        #endregion

        #region File-Based-Routing

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DistributeMessagesUsingFileBasedEndpointInstanceMapping("routes.xml");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        var sequenceId = 0;
        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var command = new DoSomething
            {
                SequenceId = ++sequenceId
            };
            await endpointInstance.Send(command)
                .ConfigureAwait(false);
            Console.WriteLine($"Message {command.SequenceId} Sent");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}