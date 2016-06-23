namespace Core6.Errors.FirstLevel
{
    using NServiceBus;

    public class FlrCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region FlrCodeFirstConfiguration
            endpointConfiguration.FirstLevelRetries().NumberOfRetries(3);
            #endregion
        }
    }
}