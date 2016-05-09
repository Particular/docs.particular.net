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
        Console.Title = "Samples.AzureBlobStorageDataBus.Receiver";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.AzureBlobStorageDataBus.Receiver");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UseDataBus<AzureDataBus>()
            .ConnectionString("UseDevelopmentStorage=true");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}