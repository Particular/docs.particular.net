using System;
using System.Threading.Tasks;
using NServiceBus;
using System.Configuration;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomDistributionStrategy.Server1";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomDistributionStrategy.Server");

        #region Server-Set-InstanceId

        var discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.MakeInstanceUniquelyAddressable(discriminator);

        #endregion

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