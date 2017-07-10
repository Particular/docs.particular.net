using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using ServiceControl.TransportAdapter;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ServiceControl.SqlServerTransportAdapter.Adapter";
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<SqlServerTransport, MsmqTransport>("ServiceControl.SqlServer.Adapter");

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {
                transport.ConnectionString(@"Data Source=.\SqlExpress;Initial Catalog=shipping;Integrated Security=True;Max Pool Size=100;Min Pool Size=10");
                //HACK: SQLServer expects this to be present. Will be solved in SQL 3.1
                transport.GetSettings().Set<EndpointInstances>(new EndpointInstances());
            });

        #endregion

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                //HACK: Latest MSMQ requires this setting. To be moved to the transport adapter core.
                transport.GetSettings().Set("errorQueue", "poison");
            });

#pragma warning restore 618

        var adapter = TransportAdapter.Create(transportAdapterConfig);

        await adapter.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to shutdown adapter.");
        Console.ReadLine();

        await adapter.Stop()
            .ConfigureAwait(false);
    }
}