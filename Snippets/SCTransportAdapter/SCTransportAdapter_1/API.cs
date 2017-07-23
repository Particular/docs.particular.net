using System;
using NServiceBus.Settings;
using NServiceBus.Transport;
using ServiceControl.TransportAdapter;

namespace SCTransportAdapter_0
{
    using System.Threading.Tasks;

    public class API
    {
        string BrokerOneConnectionString = "Broker One connection string";
        string BrokerTwoConnectionString = "Broker Two connection string";
        string ScConnectionString = "SC connection string";
        string ConnectionString = "Common connection string";
        string OtherTransportConnectionString = "Other transport connection string";

        void MixedTransports()
        {
            #region MixedTransports

            var transportAdapterConfig =
                new TransportAdapterConfig<MyOtherTransport, MyTransport>("MyOtherTransport");

            transportAdapterConfig.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(OtherTransportConnectionString);
                });

            transportAdapterConfig.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(ConnectionString);
                });

            #endregion
        }

        void AdvancedFeatures()
        {
            #region AdvancedFeatures

            var transportAdapterConfig = new TransportAdapterConfig<MyTransport, MyTransport>("TransportAdapter")
            {
                EndpointSideAuditQueue = "audit_in",
                EndpointSideErrorQueue = "error_in",
                ServiceControlSideControlQueue = "control_in"
            };

            transportAdapterConfig.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(ConnectionString);
                    transportExtensions.UseSpecificRouting();
                });

            transportAdapterConfig.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(ConnectionString);
                });

            #endregion

        }

        void MultipleBrokers()
        {
            #region MultiInstance

            var transportAdapterConfigOne = new TransportAdapterConfig<MyTransport, MyTransport>("One");
            transportAdapterConfigOne.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(BrokerOneConnectionString);
                });
            transportAdapterConfigOne.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(ScConnectionString);
                });

            var transportAdapterConfigTwo = new TransportAdapterConfig<MyTransport, MyTransport>("Two");
            transportAdapterConfigTwo.CustomizeEndpointTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(BrokerTwoConnectionString);
                });
            transportAdapterConfigTwo.CustomizeServiceControlTransport(
                customization: transportExtensions =>
                {
                    transportExtensions.ConnectionString(ScConnectionString);
                });

            #endregion
        }

        void Configuration(TransportAdapterConfig<MyTransport, MyTransport> transportAdapterConfig)
        {

            #region AuditQueues

            transportAdapterConfig.EndpointSideAuditQueue = "audit_in";
            transportAdapterConfig.ServiceControlSideAuditQueue = "audit_out";

            #endregion

            #region ErrorQueues

            transportAdapterConfig.EndpointSideErrorQueue = "error_in";
            transportAdapterConfig.ServiceControlSideErrorQueue = "error_out";

            #endregion

            #region ControlQueues

            transportAdapterConfig.EndpointSideControlQueue = "control_in";
            transportAdapterConfig.ServiceControlSideControlQueue = "control_out";

            #endregion

            #region PoisonQueue

            transportAdapterConfig.PoisonMessageQueue = "poison_queue";

            #endregion

            #region ControlRetries

            transportAdapterConfig.ControlForwardingImmediateRetries = 10;

            #endregion

            #region RetryRetries

            transportAdapterConfig.RetryForwardingImmediateRetries = 10;

            #endregion
        }

        async Task Lifecycle()
        {
            #region Lifecycle

            var config = new TransportAdapterConfig<MyTransport, MyOtherTransport>("Adapter");

            var adapter = TransportAdapter.Create(config);

            //Starting up
            await adapter.Start().ConfigureAwait(false);

            //Shutting down
            await adapter.Stop().ConfigureAwait(false);

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
