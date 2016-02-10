using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

class Program
{

    static void Main()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.Scaleout.Worker");
        #region Distributor-InstanceId
        endpointConfiguration.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);
        #endregion
        endpointConfiguration.EnlistWithLegacyMSMQDistributor(ConfigurationManager.AppSettings["MasterNodeAddress"], ConfigurationManager.AppSettings["MasterNodeControlAddress"], 10);
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