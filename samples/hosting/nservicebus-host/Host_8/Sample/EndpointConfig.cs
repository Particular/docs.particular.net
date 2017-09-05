using System.Threading.Tasks;
using NServiceBus;

#region nservicebus-host
[EndpointName("Samples.NServiceBus.Host")]
public class EndpointConfig :
    IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
    }
}

#endregion


class Runner : IWantToRunWhenEndpointStartsAndStops
{
    public Task Start(IMessageSession session)
    {
        return Task.CompletedTask;
    }

    public Task Stop(IMessageSession session)
    {
        return Task.CompletedTask;
    }
}
