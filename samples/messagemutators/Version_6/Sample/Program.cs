using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageMutators");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.SendFailedMessagesTo("error");

        #region ComponentRegistartion
        busConfiguration.RegisterComponents(components =>
        {
            components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
            components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
        });
        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
            await Runner.Run(busContext);
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}