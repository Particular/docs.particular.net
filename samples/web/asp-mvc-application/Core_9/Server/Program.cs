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
             Console.Title = "MvcServer";
             var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.Server");
             endpointConfiguration.EnableCallbacks(makesRequests: false);
             endpointConfiguration.UseTransport(new LearningTransport());
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();

             Console.WriteLine("Press any key to exit");
             Console.ReadKey();
             return endpointConfiguration;
         });

}