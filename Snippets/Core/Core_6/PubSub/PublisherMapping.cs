namespace Core6.PubSub
{
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transports;

    class PublisherMapping
    {
        void MapPublishers(EndpointConfiguration endpointConfiguration)
        {
            #region PubSub-CodePublisherMapping

            var transport = endpointConfiguration.UseTransport<UnicastTransport>();
            transport.RegisterPublisherForType("Sales", typeof(MyEvent));
            transport.RegisterPublisherForAssembly("Sales", typeof(OtherEvent).Assembly);
            #endregion
        }

        class UnicastTransport : TransportDefinition, IMessageDrivenSubscriptionTransport
        {
            protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new System.NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }
    }

    public class MyEvent : IEvent
    {
    }

    public class OtherEvent : IEvent
    {
    }
}
