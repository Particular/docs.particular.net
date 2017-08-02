using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ServiceControl.RabbitMQAdapter.Adapter";
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<RabbitMQTransport, MsmqTransport>("ServiceControl.RabbitMQ.Adapter");

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {
                transport.ConnectionString("host=localhost");
            });

        #endregion

#pragma warning restore 618

        #region SCSideConfig

        //transportAdapterConfig.CustomizeServiceControlTransport(
        //    customization: transport =>
        //    {
                
        //    });

        #endregion

        #region PreserveReplyToAddress

        transportAdapterConfig.PreserveHeaders(
            preserveCallback: headers =>
            {
                headers[AdapterSpecificHeaders.OriginalReplyToAddress] = headers[Headers.ReplyToAddress];
            },
            restoreCallback: headers =>
            {
                headers[Headers.ReplyToAddress] = headers[AdapterSpecificHeaders.OriginalReplyToAddress];
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