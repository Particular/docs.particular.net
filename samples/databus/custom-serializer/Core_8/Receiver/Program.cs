using NServiceBus;
using Shared;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DataBus.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");

        #region ConfigureReceiverCustomDataBusSerializer

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, BsonDataBusSerializer>();
        dataBus.BasePath(@"..\..\..\..\storage");

        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}