using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
       .UseNServiceBus(x =>
         {           
             var endpointConfiguration = new EndpointConfiguration("NServiceBusEndpoint");
             endpointConfiguration.UseTransport<LearningTransport>();
             endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
             endpointConfiguration.EnableInstallers();
             endpointConfiguration.SendFailedMessagesTo("error");

             #region DisableRetries

             var recoverability = endpointConfiguration.Recoverability();

             recoverability.Delayed(
                 customizations: retriesSettings =>
                 {
                     retriesSettings.NumberOfRetries(0);
                 });
             recoverability.Immediate(
                 customizations: retriesSettings =>
                 {
                     retriesSettings.NumberOfRetries(0);
                 });

             #endregion

             return endpointConfiguration;
         }).ConfigureServices((hostContext, services) =>
         {
             Console.Title = "NServiceBusEndpoint";
             services.AddHostedService<InputLoopService>();

         });

}