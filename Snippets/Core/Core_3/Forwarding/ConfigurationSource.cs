namespace Core3.Forwarding
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region ConfigurationSourceForMessageForwarding
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(UnicastBusConfig))
            {
                var config = new UnicastBusConfig
                {
                    ForwardReceivedMessagesTo = "destinationQueue@machine"
                };

                return config as T;
            }

            // Respect app.config for other sections not defined in this method
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}
