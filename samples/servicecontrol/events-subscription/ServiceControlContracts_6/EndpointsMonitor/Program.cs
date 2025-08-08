using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;

await Host.CreateDefaultBuilder(args)
 .ConfigureServices((hostContext, services) =>
 {
 }).UseNServiceBus(x =>
 {
     Console.Title = "EndpointsMonitor";
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
 }).Build().RunAsync();