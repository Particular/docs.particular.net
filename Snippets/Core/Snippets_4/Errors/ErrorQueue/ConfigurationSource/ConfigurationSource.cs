namespace Snippets4.Errors.ErrorQueue.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region ErrorQueueConfigurationSource

    public class ConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
            {
                MessageForwardingInCaseOfFaultConfig errorConfig = new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = "error"
                };

                return errorConfig as T;
            }

            // To in app.config for other sections not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }

    #endregion
}