namespace Snippets6.PubSub
{
    using NServiceBus;

    class PublisherMapping
    {
        void MapPublishers(EndpointConfiguration endpointConfiguration)
        {
            #region PubSub-CodePublisherMapping
            endpointConfiguration.UnicastRouting().AddPublisher("Sales", typeof(MyEvent));
            endpointConfiguration.UnicastRouting().AddPublisher("Sales", typeof(OtherEvent).Assembly);
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
