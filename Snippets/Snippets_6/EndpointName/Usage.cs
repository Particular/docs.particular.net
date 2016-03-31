namespace Snippets6.EndpointName
{
    using System;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transports;

    class Usage
    {
        Usage()
        {
            #region EndpointNameCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #endregion

            #region InputQueueName

            endpointConfiguration.UseTransport<MyTransport>().AddAddressTranslationException(
                new EndpointInstance("MyEndpoint"), "MyEndpoint.Messages");

            #endregion

            #region InputQueueOverrideRouting

            endpointConfiguration.UnicastRouting().RouteToEndpoint(typeof(MyMessage), "MyEndpoint");
            endpointConfiguration.UseTransport<MyTransport>().AddAddressTranslationException(
                new EndpointInstance("MyEndpoint"), "MyEndpoint.Messages");

            #endregion
        }

        class MyTransport : TransportDefinition
        {
            protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }

        class MyMessage
        {
        }
    }
}
