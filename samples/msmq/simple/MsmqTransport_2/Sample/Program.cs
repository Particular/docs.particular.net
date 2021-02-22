using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Msmq.Simple";
        #region ConfigureMsmqEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Msmq.Simple");
        endpointConfiguration.UseTransport(new MsmqTransport());

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(0));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}