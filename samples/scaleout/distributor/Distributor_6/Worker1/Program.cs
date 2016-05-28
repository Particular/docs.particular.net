using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Scaleout.Worker1";
        #region Workerstartup
        var endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Worker");
        var discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.ScaleOut().InstanceDiscriminator(discriminator);
        var masterNodeAddress = ConfigurationManager.AppSettings["MasterNodeAddress"];
        var masterNodeControlAddress = ConfigurationManager.AppSettings["MasterNodeControlAddress"];
        endpointConfiguration.EnlistWithLegacyMSMQDistributor(masterNodeAddress, masterNodeControlAddress, 10);
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        Run(endpointConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(EndpointConfiguration endpointConfiguration)
    {
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}