---
title: Overriding app.config settings
summary: NServiceBus uses the process config file as its default source of configuration.
reviewed: 2021-08-13
versions: '[3,7)'
component: Core
redirects:
 - nservicebus/customizing-nservicebus-configuration
---

NServiceBus uses the running process config file as its default source of configuration. Both the default behavior and the location from where the configuration is loaded can be changed. This can be done for the whole configuration or specific sections.

include: configobsolete

## Overriding an app.config section

A configuration for a specific section can be overridden using the `IProvideConfiguration<T>` interface. For example, rather than providing the `RijndaelEncryptionServiceConfig` in app.config the code below provides an alternative configuration that will be used as long as `RijndaelEncryptionServiceConfig` is found in the types scanned:

snippet: CustomConfigProvider

## Code-only configuration

If the endpoint must load its configuration from a source other than the 'app.config', an implementation of the `IConfigurationSource` interface can be registered as a custom configuration source. This approach enables retrieving the configuration from any location, such as a database or a web service.

### Registering custom configuration source

This code instructs NServiceBus to use `MyCustomConfigurationSource` as a custom configuration source:

snippet: RegisterCustomConfigSource

### Providing configuration from a custom location (other than app.config)

The `GetConfiguration` method provides data for `RijndaelEncryptionServiceConfig` directly in code, while allowing all other configuration sections to be retrieved from the config file.

snippet: CustomConfigSource
