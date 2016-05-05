---
title: Logging Profiles
summary: How to configure logging using profiles
tags:
- Logging
- Profiles
- NServiceBus.Host
redirects:
- nservicebus/logging-profiles
related:
- nservicebus/hosting/nservicebus-host/profiles
---

Logging can be configured via Profiles. However, unlike other profile behaviors, logging needs to be defined before configuring other components, even before the container. For that reason, logging configuration is kept separate from other profile behaviors.

NServiceBus has three built-in profiles for logging `Lite`, `Integration`, and `Production`.


## Default profile behavior


### In Version 5 and above

These profiles are only placeholders for logging customization. If no customization is done then the profiles have no impact on the logging defaults listed above.


### In Version 4 and below

The logging behavior configured for the three built-in profiles is shown:

| Profile     | Appender     | Threshold 
|-------------|--------------|-----
| Lite        | Console      | Info                        
| Integration | Console      | Info
| Production  | Rolling File | Info

When running under the production profile, the logs are written to 'logfile' in the same directory as the exe. The file grows to a maximum size of 1MB and then a new file is created. A maximum of 10 files is held and then the oldest file is erased. If no configuration exists, the logging threshold is Warn. Configure the logging threshold by including the following code in the application config file:

```XML
<configSections>
	<section name="Logging" type="NServiceBus.Config.Logging, NServiceBus.Core" />
</configSections>
<Logging Threshold="ERROR" />
```

For changes to the configuration to have an effect, the process must be restarted.

If different logging behaviors are required, see the next section.

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type.


## Customized logging via a profile

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type.

snippet:LoggingConfigWithProfile

Here, the host passes the instance of the class that implements `IConfigureThisEndpoint` so implementing `IWantTheEndpointConfig` is not required.

WARNING: While it is possible to have one class configure logging for multiple profile types, it is not supported to have more than one class configure logging for the same profile. NServiceBus can allow only one of these classes for all profile types passed in the command-line.