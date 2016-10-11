﻿using System;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "ClientUI";

            var endpointConfig = new EndpointConfiguration("ClientUI");
            endpointConfig.UseTransport<MsmqTransport>();
            endpointConfig.UseSerialization<JsonSerializer>();
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfig).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
