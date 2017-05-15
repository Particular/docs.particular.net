using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void MixedTransports()
        {
            #region MixedTransports

            var config = new TransportAdapterConfig<MyOtherTransport, MyTransport>(
                "MyOtherTransport");

            config.CustomizeEndpointTransport(t =>
            {
                t.ConnectionString(OTHER_TRANSPORT_CONNECTION_STRING);
            });

            config.CustomizeServiceControlTransport(t =>
            {
                t.ConnectionString(CONNECTION_STRING);
            });

            #endregion
        }

        public void AdvancedFeatures()
        {
            #region AdvancedFeatures

            var config = new TransportAdapterConfig<MyTransport, MyTransport>(
                "TransportAdapter")
            {
                EndpointSideAuditQueue = "audit_in",
                EndpointSideErrorQueue = "error_in",
                ServiceControlSideControlQueue = "control_in"
            };

            config.CustomizeEndpointTransport(t =>
            {
                t.ConnectionString(CONNECTION_STRING);
                t.UseSpecificRouting();
            });

            config.CustomizeServiceControlTransport(t =>
            {
                t.ConnectionString(CONNECTION_STRING);
            });

            #endregion

        }

        public void MultipleBrokers()
        {
            #region MultiInstance

            var aConfig = new TransportAdapterConfig<MyTransport, MyTransport>("A");
            aConfig.CustomizeEndpointTransport(t =>
            {
                t.ConnectionString(BROKER_A_CONNECTION_STRING);
            });
            aConfig.CustomizeServiceControlTransport(t =>
            {
                t.ConnectionString(SC_CONNECTION_STRING);
            });

            var bConfig = new TransportAdapterConfig<MyTransport, MyTransport>("B");
            bConfig.CustomizeEndpointTransport(t =>
            {
                t.ConnectionString(BROKER_B_CONNECTION_STRING);
            });
            bConfig.CustomizeServiceControlTransport(t =>
            {
                t.ConnectionString(SC_CONNECTION_STRING);
            });

            #endregion
        }

        public void Configuration(TransportAdapterConfig<MyTransport, MyTransport> config)
        {

            #region AuditQueues

            config.EndpointSideAuditQueue = "audit_in"; //Default: audit
            config.ServiceControlSideAuditQueue = "audit_out"; //Default: audit

            #endregion

            #region ErrorQueues

            config.EndpointSideErrorQueue = "error_in"; //Default: error
            config.ServiceControlSideErrorQueue = "error_out"; //Default: error

            #endregion

            #region ControlQueues

            config.EndpointSideControlQueue = "control_in"; //Default: Particular.ServiceControl
            config.ServiceControlSideControlQueue = "control_out"; //Default: Particular.ServiceControl

            #endregion

            #region PoisonQueue

            config.PoisonMessageQueue = "poison_queue"; //Default poisin

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
