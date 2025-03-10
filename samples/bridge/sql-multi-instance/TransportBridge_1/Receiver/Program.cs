using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
#pragma warning disable 618

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             Console.Title = "MultiInstanceReceiver";

         }).UseNServiceBus(x =>
         {
             #region ReceiverConfiguration
             var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
             var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
             transport.ConnectionString(ConnectionString);
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.EnableInstallers();
             #endregion

             SqlHelper.EnsureDatabaseExists(ConnectionString);
             Console.WriteLine("Waiting for Order messages from the Sender");
             return endpointConfiguration;
         });

    //for local instance or SqlExpress
    const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlMultiInstanceReceiver;Trusted_Connection=True;MultipleActiveResultSets=true";

   // const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

}
