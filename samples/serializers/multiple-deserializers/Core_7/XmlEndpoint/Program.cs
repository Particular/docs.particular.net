using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.MultipleDeserializers.XmlEndpoint";
        #region configXml
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.XmlEndpoint");
        endpointConfiguration.UseSerialization<XmlSerializer>();
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