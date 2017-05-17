using System;
using NServiceBus.Settings;
using NServiceBus.Transport;
using ServiceControl.TransportAdapter;

namespace SCTransportAdapter_0
{
    public class API
    {
        const string BROKER_A_CONNECTION_STRING = "Broker A connection string";
        const string BROKER_B_CONNECTION_STRING = "Broker B connection string";
        const string SC_CONNECTION_STRING = "SC connection string";
        const string CONNECTION_STRING = "Common connection string";
        const string OTHER_TRANSPORT_CONNECTION_STRING = "Other transport connection string";

        void MixedTransports()
        {
            #region MixedTransports

            var config = new TransportAdapterConfig<MyOtherTransport, MyTransport>("MyOtherTransport");

            config.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(OTHER_TRANSPORT_CONNECTION_STRING);
                });

            config.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(CONNECTION_STRING);
                });

            #endregion
        }

        void AdvancedFeatures()
        {
            #region AdvancedFeatures

            var config = new TransportAdapterConfig<MyTransport, MyTransport>("TransportAdapter")
            {
                EndpointSideAuditQueue = "audit_in",
                EndpointSideErrorQueue = "error_in",
                ServiceControlSideControlQueue = "control_in"
            };

            config.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(CONNECTION_STRING);
                    transportExtensions.UseSpecificRouting();
                });

            config.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(CONNECTION_STRING);
                });

            #endregion

        }

        void MultipleBrokers()
        {
            #region MultiInstance

            var aConfig = new TransportAdapterConfig<MyTransport, MyTransport>("A");
            aConfig.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(BROKER_A_CONNECTION_STRING);
                });
            aConfig.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(SC_CONNECTION_STRING);
                });

            var bConfig = new TransportAdapterConfig<MyTransport, MyTransport>("B");
            bConfig.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(BROKER_B_CONNECTION_STRING);
                });
            bConfig.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(SC_CONNECTION_STRING);
                });

            #endregion
        }

        void Configuration(TransportAdapterConfig<MyTransport, MyTransport> config)
        {

            #region AuditQueues

            // Default: audit
            config.EndpointSideAuditQueue = "audit_in";
            // Default: audit
            config.ServiceControlSideAuditQueue = "audit_out";

            #endregion

            #region ErrorQueues

            // Default: error
            config.EndpointSideErrorQueue = "error_in";
            // Default: error
            config.ServiceControlSideErrorQueue = "error_out";

            #endregion

            #region ControlQueues

            // Default: Particular.ServiceControl
            config.EndpointSideControlQueue = "control_in";
            // Default: Particular.ServiceControl
            config.ServiceControlSideControlQueue = "control_out";

            #endregion

            #region PoisonQueue

            // Default poison
            config.PoisonMessageQueue = "poison_queue";

            #endregion

            #region ControlRetries

            config.ControlForwardingImmediateRetries = 10;

            #endregion

            #region RetryRetries

            config.RetryForwardingImmediateRetries = 10;

            #endregion
        }

        public class MyTransport : TransportDefinition
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage => null;
        }



        public class MyOtherTransport : TransportDefinition
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage => null;
        }
    }
}