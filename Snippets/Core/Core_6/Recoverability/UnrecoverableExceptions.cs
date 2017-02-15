namespace Core6.Recoverability
{
    using System;
    using NServiceBus;
    using NServiceBus.Settings;

    public class UnrecoverableExceptions
    {
        void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region UnrecoverableExceptions

            var recoverability = endpointConfiguration.Recoverability();

            recoverability.AddUnrecoverableException<ArgumentNullException>();
            recoverability.AddUnrecoverableException(typeof(TimeoutException));

            #endregion
        }

        void Configure(SettingsHolder settings)
        {
            #region UnrecoverableExceptionsSettings

            settings.AddUnrecoverableException(typeof(TimeoutException));

            #endregion
        }
    }
}