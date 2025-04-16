using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;

public class Program
{
    //for local instance or SqlExpress
    const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlMultiInstanceSender;Trusted_Connection=True;MultipleActiveResultSets=true;Max Pool Size=100;Encrypt=false";
    //const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
       .UseNServiceBus(x =>
         {
             #region SenderConfiguration
             var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
             var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
             transport.ConnectionString(ConnectionString);
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.EnableInstallers();

             transport.Routing().RouteToEndpoint(typeof(ClientOrder), "Samples.SqlServer.MultiInstanceReceiver");
             #endregion

             SqlHelper.EnsureDatabaseExists(ConnectionString);
             Console.WriteLine("Press <enter> to send a message");
             return endpointConfiguration;
         }).ConfigureServices((hostContext, services) =>
         {
             Console.Title = "MultiInstanceSender";
             services.AddHostedService<InputLoopService>();
         });



}
