using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");

        #region ConfigureReceiverCustomDataBusSerializer

        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<JsonDataBusSerializer>(DependencyLifecycle.SingleInstance);
            });

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath(@"..\..\..\..\storage");

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}