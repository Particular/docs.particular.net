namespace Core.PubSub.Publishing;

using System.Threading.Tasks;
using NServiceBus;

class PublishAtStartup
{
    public async Task Publish(EndpointConfiguration endpointConfiguration)
    {
        #region publishAtStartup
        // Other config
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await endpointInstance.Publish(new MyEvent());

        #endregion

    }
}

public class MyEvent
{
}