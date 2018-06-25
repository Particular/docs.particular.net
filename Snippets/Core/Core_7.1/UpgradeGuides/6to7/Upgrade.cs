namespace Core7.UpgradeGuides._6to7
{
    using NServiceBus;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class Upgrade
    {

        void UseTransport(EndpointConfiguration endpointConfiguration)
        {

            #region 6to7-UseMsmqTransport

            endpointConfiguration.UseTransport<MsmqTransport>();

            #endregion
        }
    }

    class MsmqTransport:TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}