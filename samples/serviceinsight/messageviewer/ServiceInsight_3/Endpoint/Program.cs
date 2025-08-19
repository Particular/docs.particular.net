using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "CustomViewerEndpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceInsightCustomViewer.Endpoint");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");

        #region RegisterMessageEncryptor

        endpointConfiguration.RegisterMessageEncryptor();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var completeOrder = new CompleteOrder
        {
            CreditCard = "123-456-789"
        };
        await endpointInstance.SendLocal(completeOrder);

        Console.WriteLine("Message sent");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}
