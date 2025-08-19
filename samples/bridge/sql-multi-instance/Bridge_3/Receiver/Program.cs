using System;
using System.Threading.Tasks;

using NServiceBus;

class Program
{
    //for local instance or SqlExpress
    // const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=nservicebus;Trusted_Connection=True;MultipleActiveResultSets=true";
    const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

    static async Task Main()
    {
        Console.Title = "MultiInstanceReceiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
        endpointConfiguration.UseTransport(new SqlServerTransport(ConnectionString));

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.EnableInstallers();

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for Order messages from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
