namespace Core6.PubSub
{
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class PublisherMapping
    {
        void MapPublishers(EndpointConfiguration endpointConfiguration)
        {
            #region PubSub-CodePublisherMapping

            var routing = endpointConfiguration.UseTransport<UnicastTransport>().Routing();
            routing.RegisterPublisher(typeof(MyEvent), "Sales");
            routing.RegisterPublisher(typeof(OtherEvent).Assembly, "Sales");

            #endregion
        }

        class UnicastTransport :
            TransportDefinition,
            IMessageDrivenSubscriptionTransport
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new System.NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }
    }

    public class MyEvent :
        IEvent
    {
    }

    public class OtherEvent :
        IEvent
    {
    }
}