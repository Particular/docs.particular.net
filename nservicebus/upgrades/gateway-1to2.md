---
title: Upgrade Gateway from Version 1 to Version 2
summary: Instructions on how to upgrade the Gateway from Versions 1 to 2
tags:
 - upgrade
 - migration
related:
- nservicebus/gateway
- nservicebus/upgrades/5to6
---

## Extensibility

`IForwardMessagesToSites`, `IRouteMessagesToEndpoints`, and `IRouteMessagesToSites` have been deprecated and are no longer available as extension points in the gateway. To override the default HTTP channel, register custom `IChannelSender` and `IChannelReceiver` factory methods through the new extension point `configure.Gateway().ChannelFactories()` in the `EndpointConfiguration` of an endpoint. Dependency injection is not provided for these factory methods. `IChannelSender` and `IChannelReceiver` implementations are also no longer automatically picked up by assembly scanning.


## Concurrency config

`NumberOfWorkerThreads` is now deprecated as a parameter for channels in the endpoint config file. Use `MaxConcurrency` to set the maximum number of messages that should be processed at any given time by the gateway instead.

snippet: 1to2GatewayConfig


## Notifications

In Versions 2 and above the gateway does not provide any error notifications. When an error occurs during sending of a message to other sites, the message will be retried and possibly moved to the error queue. The user will not be notified about these events.

Note that in Version 1, when the user [subscribes to error notifications ](/nservicebus/errors/subscribing-to-error-notifications.md) they receive notification in the situation described above.