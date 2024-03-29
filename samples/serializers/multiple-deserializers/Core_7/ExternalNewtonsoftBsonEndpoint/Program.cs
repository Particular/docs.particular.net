﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "NewtonsoftBsonEndpoint";
        #region configExternalNewtonsoftBson
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ExternalNewtonsoftBsonEndpoint");
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.ReaderCreator(stream => new BsonDataReader(stream));
        serialization.WriterCreator(stream => new BsonDataWriter(stream));
        serialization.ContentTypeKey("NewtonsoftBson");
        endpointConfiguration.RegisterOutgoingMessageLogger();

        #endregion
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var message = MesasgeBuilder.BuildMessage();
        await endpointInstance.Send("Samples.MultipleDeserializers.ReceivingEndpoint", message);
        Console.WriteLine("Order Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}