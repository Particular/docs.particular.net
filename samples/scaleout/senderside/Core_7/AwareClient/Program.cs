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
        Console.Title = "Samples.SenderSideScaleOut.AwareClient";
        var endpointConfiguration = new EndpointConfiguration("Samples.SenderSideScaleOut.AwareClient");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routingSettings = transport.Routing();

        #region Logical-Routing

        routingSettings.RouteToEndpoint(
            messageType: typeof(DoSomething),
            destination: "Samples.SenderSideScaleOut.Server");

        #endregion

        #region File-Based-Routing

        var instanceMappingFile = routingSettings.InstanceMappingFile();
        instanceMappingFile.FilePath("routes.xml");

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
            var command = new DoSomething { SequenceId = ++sequenceId };
            await endpointInstance.Send(command)
                .ConfigureAwait(false);
            Console.WriteLine($"Message {command.SequenceId} Sent");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}