using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomDistributionStrategy.Server2";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomDistributionStrategy.Server");
        var discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.MakeInstanceUniquelyAddressable(discriminator);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}