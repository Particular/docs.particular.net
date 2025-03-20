using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

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
         }).UseNServiceBus(x =>
         {
             Console.Title = "ContentManagement";
             var endpointConfiguration = new EndpointConfiguration("Store.ContentManagement");
             endpointConfiguration.ApplyCommonConfiguration(routing =>
             {
                 routing.RouteToEndpoint(typeof(Store.Messages.RequestResponse.ProvisionDownloadRequest), "Store.Operations");
             });


             Console.WriteLine("Press any key to exit");
             Console.ReadKey();
             return endpointConfiguration;
         });
}
