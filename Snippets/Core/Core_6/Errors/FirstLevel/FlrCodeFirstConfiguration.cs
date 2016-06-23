namespace Core6.Errors.FirstLevel
{
    using NServiceBus;

    public class FlrCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region FlrCodeFirstConfiguration

            var firstLevelRetries = endpointConfiguration.FirstLevelRetries();
            firstLevelRetries.NumberOfRetries(3);
            #endregion
        }
    }
}