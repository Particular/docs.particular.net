namespace Core6.EndpointName
{
    using System;
    using NServiceBus;
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

            endpointConfiguration.OverrideLocalAddress("MyEndpoint.Messages");

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
