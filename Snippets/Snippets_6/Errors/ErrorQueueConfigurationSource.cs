﻿using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using System.Configuration;
using NServiceBus;

#region ErrorQueueConfigurationSource
public class ErrorQueueConfigurationSource : IConfigurationSource
{
    public T GetConfiguration<T>() where T : class, new()
    {
        if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
        {
            var errorConfig = new MessageForwardingInCaseOfFaultConfig
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

public class InjectProvideErrorQueueConfiguration 
{
    public void Init()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        #region UseCustomConfigurationSourceForErrorQueueConfig
        busConfiguration.CustomConfigurationSource(new ErrorQueueConfigurationSource());
        #endregion
    }
}