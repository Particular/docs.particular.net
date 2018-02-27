using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.SqsTransportAdapter.Adapter";
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<SqsTransport, MsmqTransport>("ServiceControl.SQS.Adapter");

        #endregion

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(transport =>
        {
            transport.S3("bucketname", "my/key/prefix");
        });

        #endregion

        var adapter = TransportAdapter.Create(transportAdapterConfig);

        await adapter.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to shutdown adapter.");
        Console.ReadLine();

        await adapter.Stop()
            .ConfigureAwait(false);
    }
}