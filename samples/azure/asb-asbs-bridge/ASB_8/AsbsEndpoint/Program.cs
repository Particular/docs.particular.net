﻿using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.AsbEndpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.AsbsEndpoint");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.AddDeserializer<XmlSerializer>();
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);

        #region connect-asb-side-of-bridge

        var routing = transport.Routing();
        var bridge = routing.ConnectToBridge("Bridge-ASBS");

        #endregion

        #region subscribe-to-event-via-bridge

        // bridge.RegisterPublisher(typeof(MyEvent), "Samples.Azure.ServiceBus.AsbEndpoint");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}