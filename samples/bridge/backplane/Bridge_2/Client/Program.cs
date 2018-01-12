using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.Backplane.Client");

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionStrings.Blue);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        #region ClientBridgeConfig

        var bridge = transport.Routing().ConnectToBridge("Samples.Bridge.Backplane.Bridge.Blue");
        bridge.RouteToEndpoint(typeof(MyMessage), "Samples.Bridge.Backplane.Server");
        //bridge.RegisterPublisher(typeof(MyEvent), "Samples.Bridge.Backplane.Server");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <enter> to send a message");
        while (true)
        {
            Console.ReadLine();
            var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            var message = new MyMessage
            {
                Id = id
            };
            await endpointInstance.Send(message)
                .ConfigureAwait(false);
        }
    }
}