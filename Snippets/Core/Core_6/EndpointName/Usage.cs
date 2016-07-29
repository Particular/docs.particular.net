namespace Core6.EndpointName
{
    using System;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class Usage
    {
        void EndpointNameCode()
        {
            #region EndpointNameCode

            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #endregion
        }
        void InputQueueName(EndpointConfiguration endpointConfiguration)
        {
            #region InputQueueName

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.AddAddressTranslationException(
                new EndpointInstance("MyEndpoint"), "MyEndpoint.Messages");

            #endregion
        }

        void InputQueueOverrideRouting(EndpointConfiguration endpointConfiguration)
        {
            #region InputQueueOverrideRouting

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(MyMessage), "MyEndpoint");
            transport.AddAddressTranslationException(
                new EndpointInstance("MyEndpoint"), "MyEndpoint.Messages");

            #endregion
        }

        class MyTransport :
            TransportDefinition
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
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
