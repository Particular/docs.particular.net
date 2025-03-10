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
             Console.Title = "Operations";
             var endpointConfiguration = new EndpointConfiguration("Store.Operations");
             endpointConfiguration.ApplyCommonConfiguration();
             Console.WriteLine("Press any key to exit");
             Console.ReadKey();

             return endpointConfiguration;
         });

}
