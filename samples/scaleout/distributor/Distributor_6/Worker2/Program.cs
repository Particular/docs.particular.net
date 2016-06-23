using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Routing.Legacy;
using NServiceBus.Support;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Scaleout.Worker2";
        const string endpointName = "Samples.Scaleout.Worker";
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        var instanceId = ConfigurationManager.AppSettings["InstanceId"];
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.AddAddressTranslationException(
            new EndpointInstance(endpointName).AtMachine(RuntimeEnvironment.MachineName),
            $"{endpointName}-{instanceId}");
        transport.RegisterPublisherForType("Samples.Scaleout.Sender", typeof(OrderPlaced));
        var masterNodeAddress = ConfigurationManager.AppSettings["MasterNodeAddress"];
        var masterNodeControlAddress = ConfigurationManager.AppSettings["MasterNodeControlAddress"];
        endpointConfiguration.EnlistWithLegacyMSMQDistributor(masterNodeAddress, masterNodeControlAddress, 10);
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