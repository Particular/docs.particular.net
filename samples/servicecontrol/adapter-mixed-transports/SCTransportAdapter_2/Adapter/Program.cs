using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.TransportAdapter;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ServiceControl.Adapter";
        #region AdapterTransport

        var transportAdapterConfig = new TransportAdapterConfig<SqlServerTransport, LearningTransport>("Samples.ServiceControl.Adapter");

        #endregion

#pragma warning disable 618

        #region EndpointSideConfig

        transportAdapterConfig.CustomizeEndpointTransport(
            customization: transport =>
            {              
                // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=transport_adapter;Integrated Security=True;Encrypt=false;Max Pool Size=100;Min Pool Size=10
                var connectionString = @"Server=localhost,1433;Initial Catalog=transport_adapter;User Id=SA;Password=yourStrong(!)Password;Encrypt=false;Max Pool Size=100;Min Pool Size=10";

                transport.ConnectionString(connectionString);
            });

        #endregion

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