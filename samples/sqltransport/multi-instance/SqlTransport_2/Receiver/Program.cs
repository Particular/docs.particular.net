using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.MultiInstanceReceiver");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(ConnectionInfoProvider.GetConnection);
        transport.ConnectionString(ConnectionInfoProvider.DefaultConnectionString);
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #endregion
        SqlHelper.EnsureDatabaseExists(ConnectionInfoProvider.DefaultConnectionString);


        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.WriteLine("Waiting for Order messages from the Sender");
            Console.ReadKey();
        }
    }

}