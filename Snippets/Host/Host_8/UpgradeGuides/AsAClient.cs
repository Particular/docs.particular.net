namespace Host_8.UpgradeGuides
{
    using System;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class AsAClient
    {
        public AsAClient()
        {
            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #region 7to8AsAClient

            endpointConfiguration.PurgeOnStartup(true);
            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.None);

            endpointConfiguration.Recoverability().Delayed(delayed => delayed.NumberOfRetries(0));

            #endregion
        }

        class MyTransport : TransportDefinition
        {
            public override string ExampleConnectionStringForErrorMessage { get; }

            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new NotImplementedException();
            }
        }
    }
}