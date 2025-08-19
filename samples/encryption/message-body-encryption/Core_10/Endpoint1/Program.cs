using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Samples.MessageBodyEncryption.Endpoint1");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region RegisterMessageEncryptor

        endpointConfiguration.RegisterMessageEncryptor();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var completeOrder = new CompleteOrder
        {
            CreditCard = "123-456-789"
        };
        await endpointInstance.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder);
        Console.WriteLine("Message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
