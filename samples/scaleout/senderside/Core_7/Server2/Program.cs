using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SenderSideScaleOut.Server2";
        var endpointConfiguration = new EndpointConfiguration("Samples.SenderSideScaleOut.Server");
        var discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.MakeInstanceUniquelyAddressable(discriminator);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}