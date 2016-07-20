namespace Core5.Forwarding
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
            // To Provide UnicastBusConfig
            if (typeof(T) == typeof(UnicastBusConfig))
            {
                var forwardingConfig = new UnicastBusConfig
                {
                    ForwardReceivedMessagesTo = "destinationQueue@machine"
                };

                return forwardingConfig as T;
            }

            // To in app.config for other sections not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}
