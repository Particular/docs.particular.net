namespace Core7.PublishSubscribe
{
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class DisablePublishing
    {
        void DisablePublishingConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region DisablePublishing
            var transportConfiguration = endpointConfiguration.UseTransport<TransportDefinition>();
            transportConfiguration.DisablePublishing();
            #endregion
        }

        class TransportDefinition : NServiceBus.Transport.TransportDefinition, IMessageDrivenSubscriptionTransport
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new System.NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }
    }
}