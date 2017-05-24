namespace Core6.UpgradeGuides._5to6.Recoverability
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

        void DisableImmediateRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityDisableImmediateRetries

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                });

            #endregion
        }

        void DisableDelayedRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityDisableDelayedRetries

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(0);
                });

            #endregion
        }

    }
}