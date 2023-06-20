﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge.Endpoint";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.Bridge.Endpoint");

        endpointConfiguration.UseTransport(new LearningTransport
        {
            StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
        });        

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: immediate =>
            {
                immediate.NumberOfRetries(0);
            });
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableMetrics().SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to exit");
        Console.WriteLine("Press 'o' to send a message");
        Console.WriteLine("Press 'f' to toggle simulating of message processing failure");
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            var lowerInvariant = char.ToLowerInvariant(key.KeyChar);
            if (lowerInvariant == 'o')
            {
                var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var message = new MyMessage
                {
                    Id = id,
                };
                await endpointInstance.SendLocal(message)
                    .ConfigureAwait(false);
            }
            if (lowerInvariant == 'f')
            {
                FailureSimulator.Enabled = !FailureSimulator.Enabled;
                Console.WriteLine("Failure simulation is now turned " + (FailureSimulator.Enabled ? "on" : "off"));
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}