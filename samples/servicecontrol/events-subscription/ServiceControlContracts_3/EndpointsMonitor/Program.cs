using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "EndpointsMonitor";
        var endpointConfiguration = new EndpointConfiguration("EndpointsMonitor");
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

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


        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to finish.");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
