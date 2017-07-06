namespace Core7.UpgradeGuides._6to7
{
    using NServiceBus;

    class Upgrade
    {

        void UseTransport(EndpointConfiguration endpointConfiguration)
        {

            #region 6to7-UseMsmqTransport

            endpointConfiguration.UseTransport<MsmqTransport>();

            #endregion
        }

    }
}