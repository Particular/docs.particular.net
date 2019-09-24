---
title: Overriding app.config settings
summary: NServiceBus uses the process config file as its default source of configuration.
reviewed: 2019-07-17
versions: '[3,7)'
component: Core
redirects:
 - nservicebus/customizing-nservicebus-configuration
---

NServiceBus uses the running process config file as its default source of configuration. NServiceBus allows changing the default behavior and customizing the location from where the configuration is loaded. This can be done for the whole configuration or a specific sections.

include: configobsolete


## Overriding App.Config section

A configuration for a specific section can be overridden using the `IProvideConfiguration<T>` interface. For example, rather than providing the `RijndaelEncryptionServiceConfig` in app.config the code below provides an alternative configuration that will be used as long as `RijndaelEncryptionServiceConfig` is found in the types scanned: 

snippet: CustomConfigProvider


## Code only configuration

If the endpoint needs to load its configuration from some other source than the 'app.config', an implementation of `IConfigurationSource` interface can be registered as a custom configuration source. This approach enables retrieving the configuration from any location: a database, a web service, etc..


### Registering custom configuration source

This code instructs NServiceBus to use `MyCustomConfigurationSource` as custom configuration source:

snippet: RegisterCustomConfigSource

### Provide configuration from custom location (not app.config)

`GetConfiguration` method provides data for `RijndaelEncryptionServiceConfig` directly in code, while allowing all other configuration sections to be retrieved from the config file.

snippet: CustomConfigSource
