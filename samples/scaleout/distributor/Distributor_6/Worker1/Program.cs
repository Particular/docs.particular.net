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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Worker");
        string discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.ScaleOut().InstanceDiscriminator(discriminator);
        string masterNodeAddress = ConfigurationManager.AppSettings["MasterNodeAddress"];
        string masterNodeControlAddress = ConfigurationManager.AppSettings["MasterNodeControlAddress"];
        endpointConfiguration.EnlistWithLegacyMSMQDistributor(masterNodeAddress, masterNodeControlAddress, 10);
        #endregion
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        Run(endpointConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(EndpointConfiguration endpointConfiguration)
    {
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}