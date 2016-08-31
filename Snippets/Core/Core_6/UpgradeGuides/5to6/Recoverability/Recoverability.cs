namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    public class Recoverability
    {
        void ConfigureRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityCodeFirstApi

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(3);
                });
            recoverability.Delayed(
                customizations: delayed =>
                {
                    var numberOfRetries = delayed.NumberOfRetries(5);
                    numberOfRetries.TimeIncrease(TimeSpan.FromSeconds(30));
                });

            #endregion
        }
        void XmlReplacementImmediate(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-configureImmediateRetriesViaCode

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(2);
                });

            #endregion
        }
        void XmlReplacementDelayed(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-configureDelayedRetriesViaCode

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(3);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(10));
                });

            #endregion
        }

        void DisableRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityDisableRetries

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                });
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(0);
                });

            #endregion
        }

        void DisableLegacyRetriesSatellite(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-DisableLegacyRetriesSatellite

            endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();

            #endregion
        }
    }
}