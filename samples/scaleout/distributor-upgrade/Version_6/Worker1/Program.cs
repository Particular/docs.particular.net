using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

class Program
{

    static void Main()
    {
        EndpointConfiguration busConfiguration = new EndpointConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Worker");
        busConfiguration.ScaleOut().InstanceDiscriminator(ConfigurationManager.AppSettings["InstanceId"]);
        busConfiguration.EnlistWithLegacyMSMQDistributor(ConfigurationManager.AppSettings["DistributorAddress"],
            ConfigurationManager.AppSettings["DistributorControlAddress"],
            10);
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EnableInstallers();

        Run(busConfiguration).GetAwaiter().GetResult();
    }

    private static async Task Run(EndpointConfiguration busConfiguration)
    {
        var endpoint = await Endpoint.Start(busConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}