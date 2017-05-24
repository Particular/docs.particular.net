#pragma warning disable 618
namespace Core_7.UpgradeGuides._6to7.ErrorHandling
{
    using System.Configuration;
    using NServiceBus;

    public class ConfigurationChanges
    {

        void ProvideConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7ErrorQueueConfigurationProvider
            endpointConfiguration.SendFailedMessagesTo("error");

            #endregion
        }

        void AppConfig(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7configureErrorQueueReadAppSettings
            var appSettings = ConfigurationManager.AppSettings;
            var errorQueue = appSettings.Get("errorQueue");
            endpointConfiguration.SendFailedMessagesTo(errorQueue);
            #endregion
        }

        void ConfigurationSource(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7UseCustomConfigurationSourceForErrorQueueConfigNew
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion
        }
    }
}


