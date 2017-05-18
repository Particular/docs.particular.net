﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.Azure.ServiceBus.Client";

        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.Client");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.UseForwardingTopology();

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.DisableLegacyRetriesSatellite();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press 'enter' to send a message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            #region request-message

            var message = new LongProcessingRequest
            {
                Id = Guid.NewGuid(),
                // set to a longer period of time to emulate longer processing
                EstimatedProcessingTime = Constants.EstimatedProcessingTime
            };

            #endregion

            await endpointInstance.Send("Samples.Azure.ServiceBus.Server", message)
                .ConfigureAwait(false);

            Console.WriteLine($"LongProcessingRequest with ID {message.Id} and estimated processing time {message.EstimatedProcessingTime} sent.");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}