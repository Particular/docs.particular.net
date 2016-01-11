---
title: Host Profile Logging
summary: Illustrates how to configure logging using the profile functionality of the NServiceBus host.
tags:
- Logging
related:
- nservicebus/logging
---


## Code walk-through

Illustrates how to configure logging using the [profiles](/nservicebus/hosting/nservicebus-host/profiles.md) functionality of the NServiceBus host.


### Change the default profile

The startup action for this sample is configured to use the Lite profile.

snippet: specific-profile


### Logging helper

This is a simple helper that takes in a threshold as a parameter and configures logging based on that parameter.

snippet:LoggingHelper
 

### Profile Handlers

Inside the `LoggingHandlers` directory there are `IConfigureLoggingForProfile`s handlers for each of the profiles. Note that they all implement `IConfigureLoggingForProfile<T>`.


#### Integration

snippet:IntegrationHandler


#### Lite

snippet:LiteHandler


#### Production

snippet: ProductionHandler