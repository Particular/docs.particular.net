using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Versioning.V2.Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.Versioning.V2.Receiver");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        var transport = endpointConfiguration.UseTransport(new MsmqTransport());
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
                                             .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
                              .ConfigureAwait(false);
    }
}