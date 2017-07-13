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
        Console.Title = "Worker.1";

        var endpointConfiguration = new EndpointConfiguration("Samples.Scaleout.Worker");
        endpointConfiguration.OverrideLocalAddress("Samples.Scaleout.Worker-1");

        #region Enlisting

        var appSettings = ConfigurationManager.AppSettings;
        endpointConfiguration.EnlistWithLegacyMSMQDistributor(
            masterNodeAddress: appSettings["DistributorAddress"],
            masterNodeControlAddress: appSettings["DistributorControlAddress"],
            capacity: 1);

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: settings =>
            {
                settings.NumberOfRetries(0);
            });
        recoverability.Delayed(
            customizations: settings =>
            {
                var numberOfRetries = settings.NumberOfRetries(2);
                numberOfRetries.TimeIncrease(TimeSpan.FromSeconds(2));
            });
        endpointConfiguration.EnableInstallers();
        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningMessagesAs(
            type =>
            {
                return type.GetInterfaces().Contains(typeof(IMessage));
            });

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