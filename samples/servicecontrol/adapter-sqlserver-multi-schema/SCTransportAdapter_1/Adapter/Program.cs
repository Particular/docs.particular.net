using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using NServiceBus.Transport.SQLServer;
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

        var transportAdapterConfig = new TransportAdapterConfig<SqlServerTransport, SqlServerTransport>("ServiceControl.SqlServer.Adapter");

        #endregion

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {
                transport.ConnectionString(
                    @"Data Source=.\SqlExpress;Initial Catalog=nservicebus;Integrated Security=True;Max Pool Size=100;Min Pool Size=10");

                //Use custom schema
                transport.DefaultSchema("adapter");

                //Necassary to correctly route retried messages because
                //SQL Server transport 2.x did not include schema information in the address
                transport.UseSchemaForQueue("Samples.ServiceControl.SqlServerTransportAdapter.Shipping", "shipping");

                //HACK: SQLServer expects this to be present. Will be solved in SQL 3.1
                transport.GetSettings().Set<EndpointInstances>(new EndpointInstances());
            });

        #endregion

        #region SCSideConfig

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                transport.ConnectionString(
                    @"Data Source=.\SqlExpress;Initial Catalog=ServiceControl;Integrated Security=True;Max Pool Size=100;Min Pool Size=10");

                //HACK: SQLServer expects this to be present. Will be solved in SQL 3.1
                var settings = transport.GetSettings();
                settings.Set<EndpointInstances>(new EndpointInstances());
            });

        #endregion

        #region ControlQueueOverride

        transportAdapterConfig.ServiceControlSideControlQueue = "Particular.ServiceControl.SQL";

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