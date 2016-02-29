namespace Snippets6.PubSub
{
    using NServiceBus;
    using NServiceBus.Routing;

    class PublisherMapping
    {
        public void MapPublishers()
        {
            #region PubSub-CodePublisherMapping
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.Publishers()
                .AddStatic(new EndpointName("Sales"), typeof(MyEvent));
            endpointConfiguration.Publishers()
                .AddStatic(new EndpointName("Sales"), typeof(OtherEvent).Assembly);
            #endregion
        }
    }

    public class MyEvent : IEvent
    {
    }

    public class OtherEvent : IEvent
    {
    }
}
