---
title: Overriding app.config settings
summary: NServiceBus uses the process config file as its default source of configuration.
tags:
redirects:
 - nservicebus/customizing-nservicebus-configuration
---

NServiceBus uses the process config file as its default source of configuration. The pluggability and extensibility of NServiceBus allow you to change many of its behaviors, including where it gets its configuration. This can be done across all of NServiceBus or you can choose which part of NServiceBus should get its configuration from some other source.


## Overriding App.Config section

The preferred method of overriding a specific section is to use the `IProvideConfiguration<T>` model. For example, rather than providing the RijndaelEncryptionServiceConfig in app.config, you could provide it in code:

snippet: CustomConfigProvider

Just adding the code above is enough since NServiceBus will automatically use it found in the types scanned.


## Code only configuration

If you don't want your process to have its configuration specified in the config file, you can write a class that implements `IConfigurationSource` and in it retrieve the configuration from any location you like: a database, a web service, anything. Here's how:


### Initialize the bus to use the custom configuration source

snippet: RegisterCustomConfigSource


### Define your custom configuration source to provide the configuration values instead of app.config

snippet: CustomConfigSource

The initialization code instructs NServiceBus to use a `CustomConfigurationSource`, passing in an instance of a new object: `MyCustomConfigurationSource`. Its `GetConfiguration` method provides data for `RijndaelEncryptionServiceConfig` directly in code, while allowing all other configuration sections to be retrieved from the config file.