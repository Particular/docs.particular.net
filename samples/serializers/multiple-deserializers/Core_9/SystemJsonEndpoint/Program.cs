﻿using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MultipleDeserializers.ExternalNewtonsoftJsonEndpoint";

        #region configSystemJson
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.SystemJsonEndpoint");
        var serialization = endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.RegisterOutgoingMessageLogger();

        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var message = MesasgeBuilder.BuildMessage();

        await endpointInstance.Send("Samples.MultipleDeserializers.ReceivingEndpoint", message)
            .ConfigureAwait(false);

        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}