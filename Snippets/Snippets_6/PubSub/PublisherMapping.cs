namespace Snippets6.PubSub
{
    using NServiceBus;
    using NServiceBus.Routing;

    class PublisherMapping
    {
        public void MapPublishers(EndpointConfiguration endpointConfiguration)
        {
            #region PubSub-CodePublisherMapping
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
