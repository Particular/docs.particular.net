namespace Core8.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class PublishAtStartup
    {
        public async Task Publish(EndpointConfiguration endpointConfiguration)
        {
            #region publishAtStartup
            // Other config
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            await endpointInstance.Publish(new MyEvent())
                .ConfigureAwait(false);

            #endregion

        }
    }

    public class MyEvent
    {
    }
}