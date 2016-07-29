namespace Core6.Errors.FirstLevel.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region FlrConfigurationSource
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            // To Provide Immediate Retries Config
            if (typeof(T) == typeof(TransportConfig))
            {
                var immediateRetriesConfig = new TransportConfig
                {
                    MaxRetries = 2
                };

                return immediateRetriesConfig as T;
            }

            // To in app.config for other sections not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}