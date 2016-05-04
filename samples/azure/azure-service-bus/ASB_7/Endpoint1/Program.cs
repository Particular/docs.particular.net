﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AzureServiceBus;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.Azure.ServiceBus.Endpoint1";
        #region config

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.Endpoint1");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        transport.UseTopology<ForwardingTopology>();

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press 'enter' to send a message");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                Guid orderId = Guid.NewGuid();
                Message1 message = new Message1
                {
                    Property = "Hello from Endpoint1"
                };
                await endpoint.Send("Samples.Azure.ServiceBus.Endpoint2", message);
                Console.WriteLine("Message1 sent");
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}