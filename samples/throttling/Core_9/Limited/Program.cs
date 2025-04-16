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
             Console.Title = "Limited";

             #region LimitConcurrency

             var endpointConfiguration = new EndpointConfiguration("Samples.Throttling.Limited");
             endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

             #endregion

             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.UseTransport(new LearningTransport());

             #region RegisterBehavior

             endpointConfiguration.Pipeline.Register(typeof(ThrottlingBehavior), "API throttling for GitHub");

             #endregion


             Console.WriteLine("Press any key");
             Console.ReadKey();
             return endpointConfiguration;
         });


}