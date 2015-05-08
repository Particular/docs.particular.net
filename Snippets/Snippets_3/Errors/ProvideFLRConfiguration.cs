using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using System.Configuration;
using NServiceBus;
using Snippets_4.Errors;

#region FlrConfiguration

// Create a configuration source 
public class ProvideFLRConfiguration : IConfigurationSource
{
    public T GetConfiguration<T>() where T : class, new()
    {
        //To Provide FLR Config
        if (typeof(T) == typeof(MsmqTransportConfig))
        {
            var flrConfig = new MsmqTransportConfig
            {
                MaxRetries = 2
            };

            return flrConfig as T;
        }

        // To look at the app.config for other sections that's not defined in this method, otherwise return null.
        return ConfigurationManager.GetSection(typeof(T).Name) as T;
    }
}
#endregion

public class InjectProvideFLRConfiguration
{
    public void Foo()
    {
        #region UseCustomConfigurationSourceForSLR
        Configure.With()
            .CustomConfigurationSource(new ProvideSLRConfiguration());
        #endregion
    }
}