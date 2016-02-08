using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Worker");
        #region Distributor-InstanceId
        busConfiguration.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);
        #endregion
        busConfiguration.EnlistWithLegacyMSMQDistributor(ConfigurationManager.AppSettings["MasterNodeAddress"], ConfigurationManager.AppSettings["MasterNodeControlAddress"], 10);
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        Run(busConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(BusConfiguration busConfiguration)
    {
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}