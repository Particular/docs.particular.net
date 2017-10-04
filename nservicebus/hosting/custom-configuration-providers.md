---
title: Overriding app.config settings
summary: NServiceBus uses the process config file as its default source of configuration.
reviewed: 2017-10-04
versions: '[3,7)'
component: Core
redirects:
 - nservicebus/customizing-nservicebus-configuration
---

NServiceBus uses the process config file as its default source of configuration. The extensibility of NServiceBus allow changing many of its behaviors, including where it gets its configuration. This can be done across all of NServiceBus or a specific part subset of features.

include: configobsolete



## Overriding App.Config section

The preferred method of overriding a specific section is to use the `IProvideConfiguration<T>` model. For example, rather than providing the `RijndaelEncryptionServiceConfig` in app.config:

snippet: CustomConfigProvider

Adding the code above is enough since NServiceBus will automatically use it if found in the types scanned.


## Code only configuration

If the endpoint needs to derive its configuration from somewhere other than the 'app.config', write a class that implements `IConfigurationSource` and in it retrieve the configuration from any location: a database, a web service, anything.


### Initialize the bus to use the custom configuration source

snippet: RegisterCustomConfigSource


### Define the custom configuration source to provide the configuration values instead of app.config

snippet: CustomConfigSource

The initialization code instructs NServiceBus to use a `CustomConfigurationSource`, passing in an instance of a new object: `MyCustomConfigurationSource`. Its `GetConfiguration` method provides data for `RijndaelEncryptionServiceConfig` directly in code, while allowing all other configuration sections to be retrieved from the config file.
