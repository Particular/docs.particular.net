#pragma warning disable 618
namespace Core_7.UpgradeGuides._6to7.ErrorHandling
{
    using System.Configuration;
    using NServiceBus;

    public class ConfigurationChanges
    {

        void ProvideConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7ErrorCode
            endpointConfiguration.SendFailedMessagesTo("error");

            #endregion
        }

        void AppConfig(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7ErrorReadAppSettings
            var appSettings = ConfigurationManager.AppSettings;
            var errorQueue = appSettings.Get("errorQueue");
            endpointConfiguration.SendFailedMessagesTo(errorQueue);
            #endregion
        }

    }
}


