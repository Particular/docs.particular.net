namespace Snippets6.PubSub
{
    using NServiceBus;
    using NServiceBus.Routing;

    class PublisherMapping
    {
        public void MapPublishers()
        {
            #region PubSub-CodePublisherMapping
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.Publishers().AddStatic(new EndpointName("Sales"), typeof(MyEvent));
            configuration.Publishers().AddStatic(new EndpointName("Sales"), typeof(OtherEvent).Assembly);
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
