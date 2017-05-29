#pragma warning disable 618
namespace ServiceControl.ConfigureEndpoints
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region ErrorQueueRemoteMachineConfigurationSource
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
            {
                var config = new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = "error@machinename"
                };

                return config as T;
            }

            // Respect app.config for other sections not defined in this method
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}