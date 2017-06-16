﻿using System;
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

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(transport =>
        {
            transport.EnableLegacyMultiInstanceMode(Connections.GetConnection);
            //HACK: SQLServer expects this to be present. Will be solved in SQL 3.1
            transport.GetSettings().Set<EndpointInstances>(new EndpointInstances());
        });

        #endregion

#pragma warning restore 618

        #region SCSideConfig

        transportAdapterConfig.CustomizeServiceControlTransport(
            customization: transport =>
            {
                transport.ConnectionString(
                    @"Data Source=.\SQLEXPRESS;Initial Catalog=ServiceControl;Integrated Security=True;Max Pool Size=100;Min Pool Size=10");
                //HACK: SQLServer expects this to be present. Will be solved in SQL 3.1
                transport.GetSettings().Set<EndpointInstances>(new EndpointInstances());
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