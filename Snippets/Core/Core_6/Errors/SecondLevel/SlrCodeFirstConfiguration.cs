namespace Core6.Errors.SecondLevel
{
    using System;
    using NServiceBus;

    public class SlrCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region SlrCodeFirstConfiguration

            var secondLevelRetries = endpointConfiguration.SecondLevelRetries();
            secondLevelRetries.NumberOfRetries(2);
            secondLevelRetries.TimeIncrease(TimeSpan.FromMinutes(5));
            #endregion
        }
    }
}