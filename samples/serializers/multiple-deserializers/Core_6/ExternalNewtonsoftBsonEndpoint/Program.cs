using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MultipleDeserializers.ExternalNewtonsoftBsonEndpoint";
        #region configExternalNewtonsoftBson
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ExternalNewtonsoftBsonEndpoint");
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ReaderCreator(stream => new BsonDataReader(stream));
        serialization.WriterCreator(stream => new BsonDataWriter(stream));
        serialization.ContentTypeKey("NewtonsoftBson");
        endpointConfiguration.RegisterOutgoingMessageLogger();

        #endregion
        endpointConfiguration.UseTransport<LearningTransport>();

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