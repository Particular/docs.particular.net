using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.Jil;
using NServiceBus.MessagePack;
using NServiceBus.Wire;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultipleDeserializers.ReceivingEndpoint";

        #region configAll

        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ReceivingEndpoint");

        // Xml
        endpointConfiguration.UseSerialization<XmlSerializer>();

        // External Newtonsoft Json
        var externalNewtonsoftJson = endpointConfiguration.AddDeserializer<NewtonsoftSerializer>();
        externalNewtonsoftJson.ContentTypeKey("NewtonsoftJson");

        // External Newtonsoft Bson
        var externalNewtonsoftBson = endpointConfiguration.AddDeserializer<NewtonsoftSerializer>();
        externalNewtonsoftBson.ReaderCreator(stream => new BsonReader(stream));
        externalNewtonsoftBson.WriterCreator(stream => new BsonWriter(stream));
        externalNewtonsoftBson.ContentTypeKey("NewtonsoftBson");

        // Jil
        var jil = endpointConfiguration.AddDeserializer<JilSerializer>();
        jil.ContentTypeKey("Jil");

        // Merged Newtonsoft
        endpointConfiguration.AddDeserializer<JsonSerializer>();

        // Message Pack
        endpointConfiguration.AddDeserializer<MessagePackSerializer>();

        // Wire
        endpointConfiguration.AddDeserializer<WireSerializer>();

        // register the mutator so the the message on the wire is written
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<IncomingMessageBodyWriter>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}