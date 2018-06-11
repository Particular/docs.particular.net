using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.Native.NsbEndpoint";
        #region EndpointConfiguration
        var endpointConfiguration = new EndpointConfiguration("NsbEndpoint");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        transport.ConnectionString(SqlHelper.ConnectionString);
        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");

        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}