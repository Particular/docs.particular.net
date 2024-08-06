using NServiceBus;
using Shared;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");

#pragma warning disable CS0618 // Type or member is obsolete
        #region ConfigureReceiverCustomDataBusSerializer

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, BsonDataBusSerializer>();
        dataBus.BasePath(@"..\..\..\..\storage");

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
