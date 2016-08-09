using System;
using System.Threading.Tasks;
using Jil;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Jil;
using NServiceBus.MessagePack;

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

        // External Newtonsoft
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        var externalNewtonsoft = endpointConfiguration.AddDeserializer<NewtonsoftSerializer>();
        externalNewtonsoft.Settings(settings);
        externalNewtonsoft.ContentTypeKey("NewtonsoftJson");

        // Jil
        var jil = endpointConfiguration.AddDeserializer<JilSerializer>();
        var jilOptions = new Options(
            prettyPrint: true,
            excludeNulls: true,
            includeInherited: true);
        jil.Options(jilOptions);
        jil.ContentTypeKey("Jil");

        // Merged Newtonsoft
        endpointConfiguration.AddDeserializer<NServiceBus.JsonSerializer>();

        // Message Pack
        endpointConfiguration.AddDeserializer<MessagePackSerializer>();

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