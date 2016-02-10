namespace Snippets6.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class PublishAtStartup
    {
        public async Task Publish()
        {
            #region publishAtStartup

            EndpointConfiguration configuration = new EndpointConfiguration();
            //Other config
            IEndpointInstance endpointInstance = await Endpoint.Start(configuration);
            await endpointInstance.Publish(new MyEvent());

            #endregion

        }
    }

    public class MyEvent
    {
    }
}