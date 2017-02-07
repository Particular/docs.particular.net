namespace Core4.Recoverability.Immediate.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region DisableImmediateRetriesConfigurationSource
    public class DisableConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(TransportConfig))
            {
                var config = new TransportConfig
                {
                    MaxRetries = 0
                };

                return config as T;
            }

            // Respect app.config for other sections not defined in this method
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}