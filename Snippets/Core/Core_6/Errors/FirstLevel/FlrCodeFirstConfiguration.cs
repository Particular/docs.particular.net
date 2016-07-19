namespace Core6.Errors.FirstLevel
{
    using NServiceBus;

    public class FlrCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region FlrCodeFirstConfiguration

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Immediate(immedate => immedate.NumberOfRetries(3));
            #endregion
        }
    }
}