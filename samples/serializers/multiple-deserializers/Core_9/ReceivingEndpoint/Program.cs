﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.MessageMutator;

static class Program
{
    static async Task Main()
    {
        Console.Title = "ReceivingEndpoint";

        #region configAll

        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ReceivingEndpoint");

        // Xml
        endpointConfiguration.UseSerialization<XmlSerializer>();

        // Json
        endpointConfiguration.AddDeserializer<SystemJsonSerializer>();

        // External Newtonsoft Json
        var externalNewtonsoftJson = endpointConfiguration.AddDeserializer<NewtonsoftJsonSerializer>();
        externalNewtonsoftJson.ContentTypeKey("NewtonsoftJson");

        // External Newtonsoft Bson
        var externalNewtonsoftBson = endpointConfiguration.AddDeserializer<NewtonsoftJsonSerializer>();
        externalNewtonsoftBson.ReaderCreator(stream => new BsonDataReader(stream));
        externalNewtonsoftBson.WriterCreator(stream => new BsonDataWriter(stream));
        externalNewtonsoftBson.ContentTypeKey("NewtonsoftBson");

        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterMessageMutator(new IncomingMessageBodyWriter());

        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}