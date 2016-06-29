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
        var endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Worker");
        var scaleOut = endpointConfiguration.ScaleOut();
        scaleOut.InstanceDiscriminator(ConfigurationManager.AppSettings["InstanceId"]);
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
        var endpointInstance = await Endpoint.Start(busConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}