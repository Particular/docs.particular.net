using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Encryption.Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Samples.Encryption.Endpoint2");
        endpointConfiguration.Conventions().DefiningMessagesAs(type => type.Name.Contains("Message"));
        endpointConfiguration.ConfigurationEncryption();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}