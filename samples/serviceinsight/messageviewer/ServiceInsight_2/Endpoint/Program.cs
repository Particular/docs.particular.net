using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceInsightCustomViewer.Endpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.ServiceInsightCustomViewer.Endpoint");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");

        #region RegisterMessageEncryptor

        endpointConfiguration.RegisterMessageEncryptor();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        
        var completeOrder = new CompleteOrder
        {
            CreditCard = "123-456-789"
        };
        await endpointInstance.SendLocal(completeOrder)
            .ConfigureAwait(false);
        Console.WriteLine("Message sent");
        
        Console.WriteLine("Launching platform...");
        Particular.PlatformLauncher.Launch();
        
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}