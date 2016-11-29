using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing.Legacy;

class Program
{
    static void Main()
    {
        Console.Title = "Worker.2";

        var endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Worker");
        endpointConfiguration.OverrideLocalAddress("Samples.Scaleout.Worker-2");
        endpointConfiguration.EnlistWithLegacyMSMQDistributor(
            masterNodeAddress: ConfigurationManager.AppSettings["DistributorAddress"],
            masterNodeControlAddress: ConfigurationManager.AppSettings["DistributorControlAddress"],
            capacity: 1);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.Recoverability().Immediate(i => i.NumberOfRetries(0));
        endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(2).TimeIncrease(TimeSpan.FromSeconds(2)));
        endpointConfiguration.EnableInstallers();
        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningMessagesAs(
            type =>
            {
                return type.GetInterfaces().Contains(typeof(IMessage));
            });

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