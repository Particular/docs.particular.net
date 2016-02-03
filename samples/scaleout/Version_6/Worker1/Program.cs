using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

class Program
{

    static void Main()
    {
        #region Workerstartup
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Worker");
        busConfiguration.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);
        busConfiguration.EnlistWithLegacyMSMQDistributor(ConfigurationManager.AppSettings["MasterNodeAddress"], ConfigurationManager.AppSettings["MasterNodeControlAddress"], 10);
        #endregion
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