---
title: Customizing NServiceBus Configuration
summary: NServiceBus uses the process config file as its default source of configuration.
originalUrl: http://www.particular.net/articles/customizing-nservicebus-configuration
tags:
- Configuration
- app.config
createdDate: 2013-05-21T05:33:10Z
modifiedDate: 2013-08-15T17:24:41Z
authors: []
reviewers: []
contributors: []
---

NServiceBus uses the process config file as its default source of configuration. The pluggability and extensibility of NServiceBus allow you to change many of its behaviors, including where it gets its configuration. This can be done across all of NServiceBus or you can choose which part of NServiceBus should get its configuration from some other source.

Overriding App.Config section
-----------------------------

The preferred method of overriding a specific section is to use the
[IProvideConfiguration<t>](https://github.com/NServiceBus/NServiceBus/blob/master/src/NServiceBus.Core/Config/ConfigurationSource/IConfigurationSource.cs#L23) model. For example, rather than providing the RijndaelEncryptionServiceConfig in app.config, you could provide it in code:


```C#
class InitRijndaelEncryptionServiceConfig : IProvideConfiguration<RijndaelEncryptionServiceConfig>
{
    public RijndaelEncryptionServiceConfig GetConfiguration()
    {
        return new RijndaelEncryptionServiceConfig {Key = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"};
    }
}
```

 Code only configuration
-----------------------

If you don't want your process to have its configuration specified in the config file, you can write a class that implements IConfigurationSource and in it retrieve the configuration from any location you like: a database, a web service, anything. Here's how:


```C#
// Initialize the bus to use the custom configuration source
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
{
    public void Init()
    {
        Configure.With()
            .CustomConfigurationSource(new MyCustomConfigurationSource())
            .DefaultBuilder()
            .RijndaelEncryptionService();
    }
}

// Define your custom configuration source to provide the configuration values instead of app.config
public class MyCustomConfigurationSource : IConfigurationSource
{
    public T GetConfiguration<T>() where T : class, new()
    {
        // the part you are overriding
        if (typeof(T) == typeof(RijndaelEncryptionServiceConfig))
            return new RijndaelEncryptionServiceConfig { Key = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6" } as T;
        // leaving the rest of the configuration as is:
        return ConfigurationManager.GetSection(typeof(T).Name) as T;
    }
}
```

 The initialization code instructs NServiceBus to use a CustomConfigurationSource, passing in an instance of a new object: MyCustomConfigurationSource. Its GetConfiguration method provides data for RijndaelEncryptionServiceConfig directly in code, while allowing all other configuration sections to be retrieved from the config file.

**IMPORTANT** : Add a reference to System.Configuration to use the ConfigurationManager object.

To do this when using the NServiceBus host, implement IWantCustomInitialization but this time on the class [implementing IConfigureThisEndpoint](the-nservicebus-host.md) .

