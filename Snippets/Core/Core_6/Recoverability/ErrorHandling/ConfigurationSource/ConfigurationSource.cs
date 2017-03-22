namespace Core6.Recoverability.ErrorHandling.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #pragma warning disable CS0618
    #region ErrorQueueConfigurationSource
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
    #pragma warning restore CS0618
}