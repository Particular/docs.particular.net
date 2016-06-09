namespace Core6.PubSub
{
    using NServiceBus;

    class PublisherMapping
    {
        void MapPublishers(EndpointConfiguration endpointConfiguration)
        {
            #region PubSub-CodePublisherMapping

            var routing = endpointConfiguration.UnicastRouting();
            routing.AddPublisher("Sales", typeof(MyEvent));
            routing.AddPublisher("Sales", typeof(OtherEvent).Assembly);
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
