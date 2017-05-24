#pragma warning disable 618
namespace Core_6.UpgradeGuides._6to7.ErrorHandling.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region 6to7ErrorQueueConfigurationSource
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
            {
                var config = new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = "error"
                };

                return config as T;
            }

            // Respect app.config for other sections not defined in this method
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}