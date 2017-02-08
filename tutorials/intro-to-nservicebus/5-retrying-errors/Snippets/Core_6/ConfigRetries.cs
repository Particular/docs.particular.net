using System;
using NServiceBus;

namespace Core_6
{
    public class ConfigRetries
    {
        public void Immediate(EndpointConfiguration endpointConfiguration)
        {
            #region ImmediateRetries

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                });

            #endregion
        }

        public void Delayed(EndpointConfiguration endpointConfiguration)
        {
            #region DelayedRetries

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

        public void TimeIncrease(EndpointConfiguration endpointConfiguration)
        {
            #region TimeIncrease

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                });

            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(3);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(3));
                });

            #endregion
        }
    }
}