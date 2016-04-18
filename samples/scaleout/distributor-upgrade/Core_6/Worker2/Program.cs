using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

internal class Program
{

    static void Main()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Worker");
        endpointConfiguration.ScaleOut()
            .InstanceDiscriminator(ConfigurationManager.AppSettings["InstanceId"]);
        endpointConfiguration
            .EnlistWithLegacyMSMQDistributor(
            ConfigurationManager.AppSettings["DistributorAddress"], 
            ConfigurationManager.AppSettings["DistributorControlAddress"], 
            10);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.Conventions()
            .DefiningMessagesAs(t => t.GetInterfaces().Contains(typeof(IMessage)));

        Run(endpointConfiguration).GetAwaiter().GetResult();
    }

    static async Task Run(EndpointConfiguration busConfiguration)
    {
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}