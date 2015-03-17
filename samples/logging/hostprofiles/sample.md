---
title: Host Profile Logging
summary: Illustrates how to configure logging using the profile functionality of the NServiceBus host.
tags:
- Logging
---

## Code walk-through

Illustrates how to configure logging using the [profiles](/nservicebus/hosting/nservicebus-host/profiles.md) functionality of the NServiceBus host.

### Logging helper

This is a simple helper that takes in a threshold as a parameter and configures logging based on that parameter.

<!-- import LoggingHelper -->
  
### Profile Handlers

Inside the `LoggingHandlers` directory there are `IConfigureLoggingForProfile`s handlers for each of the profiles. Note that they all implement `IConfigureLoggingForProfile<T>`.

#### Integration

<!-- import IntegrationHandler -->

#### Lite 

<!-- import LiteHandler -->

#### Production

<!-- import ProductionHandler --> 

## More Info

 * [Logging in NServiceBus](/nservicebus/logging/)