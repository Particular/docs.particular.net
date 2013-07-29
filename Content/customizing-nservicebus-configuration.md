---
layout:
title: "Customizing NServiceBus Configuration"
tags: 
origin: http://www.particular.net/Articles/customizing-nservicebus-configuration
---
NServiceBus uses the process config file as its default source of configuration. The pluggability and extensibility of NServiceBus allow you to change many of its behaviors, including where it gets its configuration. This can be done across all of NServiceBus or you can choose which part of NServiceBus should get its configuration from some other source.

Overriding in code when hosting NServiceBus yourself
----------------------------------------------------

To override only a single value of a configuration section and otherwise want the configuration section to remain in the config file, hook into the initialization process of NServiceBus using the RunCustomAction method:


    NServiceBus.Configure.With()
      .Log4Net()
      .DefaultBuilder()
      .XmlSerializer()
      .MsmqTransport()
         .IsTransactional(false)
         .PurgeOnStartup(false)
      .UnicastBus()
         .ImpersonateSender(false)
      .RunCustomAction(() => 
         Configure.Instance.Configurer.ConfigureProperty(mt => mt.Address, "someQueue")
       )
      .CreateBus()
      .Start();


See the use of the static Instance property on the Configure class to get access to the current configuration object in NServiceBus. The Configurer property provides an object for adding or overriding the configuration, and the ConfigureProperty method allows overriding a specific property of a specific type providing a custom value.

The above code sets the Address property of MsmqTransport to
"someQueue". This overrides the InputQueue property of the MsmqTransportConfig section in the config file.

Overriding in code with the NServiceBus host
--------------------------------------------

When using the <span style="background-color:Yellow;">NServiceBus Host</span>, hook into the initialization process by implementing IWantCustomInitialization, including the calls that were made in the RunCustomAction method above:


    class MsmqTransportConfigOverride : IWantCustomInitialization
    {
      public void Init()
      {
        Configure.Instance.Configurer.ConfigureProperty(mt => mt.Address, "someQueue");
      }
    }


As shown, the override code is the same. The difference is how you inject it into the initialization of NServiceBus, depending on your use of the host process.

Overriding App.Config section
-----------------------------

The preferred method of overriding a specific section is to use the
[IProvideConfiguration<t>](https://github.com/NServiceBus/NServiceBus/blob/master/src/config/NServiceBus.Config/ConfigurationSource/IConfigurationSource.cs#L23) model. Following is an example taken from the [Pub/Sub sample](https://github.com/NServiceBus/NServiceBus/blob/develop/Samples/PubSub/Subscriber1/ConfigOverride.cs):

    namespace Subscriber1
    {
        using NServiceBus.Config;
        using NServiceBus.Config.ConfigurationSource;

        //demonstrate how to override specific configuration sections
        class ConfigOverride : IProvideConfiguration
        {
            public MessageForwardingInCaseOfFaultConfig GetConfiguration()
            {
                return new MessageForwardingInCaseOfFaultConfig
                           {
                               ErrorQueue = "error"
                           };
            }
        }
    }

Replacing App.Config
--------------------

If you don't want your process to have its configuration specified in the config file, you can write a class that implements IConfigurationSource and in it retrieve the configuration from any location you like: a database, a web service, anything. Here's how:


    NServiceBus.Configure.With()
      .CustomConfigurationSource(new MyConfigSource())
      ... // rest of initialization code
    public class MyConfigSource : IConfigurationSource
    {
      public T GetConfiguration() where T : class
      {
        // the part you are overriding
        if (typeof(T) == typeof(MsmqTransportConfig))
          return new MsmqTransportConfig {InputQueue = "someQueue", /* other values */ } as T;
        // leaving the rest of the configuration as is:
        return ConfigurationManager.GetSection(typeof(T).Name) as T;
      }
    }


The initialization code instructs NServiceBus to use a CustomConfigurationSource, passing in an instance of a new object: MyConfigSource. Its GetConfiguration method provides data for MsmqTransportConfig directly in code, while allowing all other configuration sections to be retrieved from the config file.

**IMPORTANT**: Add a reference to System.Configuration to use the ConfigurationManager object.

To do this when using the NServiceBus host, implement IWantCustomInitialization but this time on the [implementing IConfigureThisEndpoint class](the-nservicebus-host).

