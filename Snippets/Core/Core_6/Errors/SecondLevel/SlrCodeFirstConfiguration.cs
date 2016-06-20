namespace Core6.Errors.SecondLevel
{
    using System;
    using NServiceBus;

    public class SlrCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region SlrCodeFirstConfiguration
            endpointConfiguration.SecondLevelRetries()
                .NumberOfRetries(2)
                .TimeIncrease(TimeSpan.FromMinutes(5));
            #endregion
        }
    }
}