using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Threading.Tasks;

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
             Console.Title = "EndpointsMonitor";

         }).UseNServiceBus(x =>
         {
             var endpointConfiguration = new EndpointConfiguration("EndpointsMonitor");
             endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
             endpointConfiguration.EnableInstallers();
             endpointConfiguration.UsePersistence<NonDurablePersistence>();

             #region ServiceControlEventsMonitorCustomErrorQueue
             endpointConfiguration.SendFailedMessagesTo("error-monitoring");
             #endregion

             var transport = endpointConfiguration.UseTransport<LearningTransport>();

             var conventions = endpointConfiguration.Conventions();
             conventions.DefiningEventsAs(
                 type =>
                 {
                     return typeof(IEvent).IsAssignableFrom(type) ||
                            // include ServiceControl events
                            type.Namespace != null &&
                            type.Namespace.StartsWith("ServiceControl.Contracts");
                 });

             return endpointConfiguration;
         });

}
