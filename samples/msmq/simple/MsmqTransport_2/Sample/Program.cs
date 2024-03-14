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
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(0));

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
