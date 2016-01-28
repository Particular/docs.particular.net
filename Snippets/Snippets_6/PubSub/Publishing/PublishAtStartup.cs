namespace Snippets6.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class PublishAtStartup
    {
        public async Task Publish()
        {
            #region publishAtStartup

            BusConfiguration busConfiguration = new BusConfiguration();
            //Other config
            IEndpointInstance endpointInstance = await Endpoint.Start(busConfiguration);
            await endpointInstance.Publish(new MyEvent());

            #endregion

        }
    }

    public class MyEvent
    {
    }
}